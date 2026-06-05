# Rendu Projet - TinyRPG (Vampire Survivors-like)

Ce document détaille les Design Patterns intégrés dans mon implémentation du projet *TinyRPG*, justifiant leur utilité.

## 1. Object Pool (Optimisation Mémoire)
**Fichiers :** `PoolManager.cs`, `IPoolable.cs`
- **Pourquoi ?** Un jeu comme Vampire Survivors génère des centaines d'ennemis, de projectiles et de gemmes d'expérience à la minute. Instancier (`Instantiate`) et détruire (`Destroy`) en continu sature la mémoire et provoque des pics de Garbage Collector qui font saccader le jeu.
- **Comment ?** Le `PoolManager` pré-instancie une quantité définie de GameObjects au démarrage, les désactive, puis les fournit sur demande. Lorsque l'entité "meurt", elle est désactivée et remise dans la file d'attente (Queue).

## 2. SOAP (Scriptable Object Architecture) & Flyweight
**Fichiers :** `EnemyData.cs`, `WeaponData.cs`
- **Pourquoi ?** Si l'on instancie 500 enemies, on ne veut pas copier 500 fois leurs statistiques de base en RAM.
- **Comment ?** Les données statiques et invariables sont stockées dans des `ScriptableObjects`. Chaque ennemi (`Enemy.cs`) conserve uniquement une référence (pointeur) vers son ScriptableObject. C'est l'application du pattern *Flyweight* (poids mouche) et de la séparation des données (Data Locality).

## 3. Alterable (Pipeline de Statistiques)
**Fichiers :** `Stat.cs`, `StatModifier.cs`
- **Pourquoi ?** Dans un jeu avec des *Level Ups* fréquents et des buffs temporaires, appliquer des modifications brutes pose un problème d'ordre mathématique (ex: `+10 ATK` puis `+10% ATK` n'est pas pareil que l'inverse) et de mémorisation lorsqu'il faut retirer le buff.
- **Comment ?** La classe `Stat` garde la valeur de base intacte. Elle possède une liste de `StatModifier` (des "Transformateurs") qui sont triés par type (Flat addition, Pourcentage additif, Pourcentage multiplicatif). La valeur finale est recalculée à la volée, en utilisant un *Dirty Flag* pour ne pas recalculer inutilement.

## 4. Proxy (Redirection de Hitbox)
**Fichiers :** `HitboxProxy.cs`, `Health.cs`
- **Pourquoi ?** Le moteur physique déclenche `OnTriggerEnter` sur l'objet qui possède le *Collider*. Or, ma logique de combat (`Health`) se situe généralement sur le composant racine. Faire des `GetComponentInParent()` ou `SendMessage()` pour retrouver les HP lors de 300 collisions par frame tue la performance.
- **Comment ?** Un `HitboxProxy` est placé sur chaque Collider. Ce script est injecté (via l'inspecteur) avec une référence directe au composant `Health`. Dès que la physique interagit avec le Proxy, ce dernier redirige immédiatement l'appel vers `Health.TakeDamage()`. Le couplage est réduit et extrêmement rapide.

## 5. Factory Method
**Fichier :** `EnemyFactory.cs`
- **Pourquoi ?** Le Spawner de vagues ne devrait pas connaître la tuyauterie interne pour assembler un ennemi (récupérer de la Pool, appliquer les Stats, activer les composants).
- **Comment ?** L'`EnemyFactory` est chargée d'abstraire la création d'ennemis. Le système de vagues demande simplement `CreateEnemy(EnemyData)`. L'usine s'occupe du reste.

## 6. State Machine (IA)
**Fichiers :** `IState.cs`, `StateMachine.cs`, `EnemySpawnState.cs`, `EnemyChaseState.cs`, `EnemyDieState.cs`
- **Pourquoi ?** Éviter les cascades de `if / else` dans l'`Update()` de l'ennemi pour savoir s'il est en train de spawner, poursuivre le joueur, ou mourir.
- **Comment ?** La logique est scindée dans des classes distinctes héritant de `IState`. L'`Enemy` possède une `StateMachine` et lui passe simplement la main lors du `Tick()`.

## 7. Strategy (Polymorphisme d'Armes)
**Fichiers :** `WeaponData.cs`, `ForwardWeaponData.cs`, `AuraWeaponData.cs`, `PlayerWeaponController.cs`
- **Pourquoi ?** Coder en dur le comportement de chaque attaque dans le `PlayerWeaponController` obligerait à créer des cascades de `if/else` (ex: "si épée, faire X ; si aura, faire Y"). C'est rigide et difficile à maintenir (violation du principe Open/Closed).
- **Comment ?** La logique de tir a été extraite de l'acteur (le joueur) pour être encapsulée dans le fichier de données de l'arme via le **Strategy Pattern**. `WeaponData` est devenu une classe abstraite qui impose une méthode `Fire()`. Le contrôleur du joueur se contente de gérer le chronomètre (Cooldown) et d'appeler `Arme.Fire()`. Chaque enfant (`ForwardWeaponData`, `AuraWeaponData`) contient sa propre implémentation unique de cette méthode. Cela permet d'ajouter une infinité de nouvelles armes aux comportements radicaux sans jamais modifier le code du joueur.

## 8. Service Locator & Injection de Dépendances (Init Method / Champ)
**Fichiers :** `ServiceLocator.cs`, `GameBootstrapper.cs`, `EnemyFactory.cs`, `WaveManager.cs`
- **Pourquoi ?** Les Singletons stricts (`Manager.Instance`) lient fortement le code et créent un couplage rigide (anti-pattern). Cependant, le `Service Locator` seul peut poser des soucis d'ordre d'exécution (race conditions au démarrage) et masquer les véritables dépendances d'une classe.
- **Comment ?** J'ai mélangé plusieurs solutions vues en cours pour une architecture optimale :
  - **Service Locator :** Utilisé uniquement pour les entités générées dynamiquement (comme le `Player` qui s'enregistre pour que les ennemis générés au runtime puissent le trouver).
  - **Injection par Champ (Slide 27) :** Le `WaveManager` demande explicitement une `EnemyFactory` via l'attribut `[SerializeField]`, rendant la dépendance visible et paramétrable dans l'éditeur.
  - **Injection par Méthode / Constructeur (Slide 28) :** L'`EnemyFactory` ne va pas chercher ses dépendances d'elle-même. Elle possède une méthode `Init(PoolManager poolManager)` qui exprime explicitement son besoin. Un script chef d'orchestre (`GameBootstrapper`) est chargé de satisfaire cette dépendance de manière totalement contrôlée au lancement du jeu.

## 9. Les limites de l'architecture : Vers la Data Locality et l'ECS
**Slides 45 à 51**
- **La question :** Est-il vraiment optimisé d'avoir des composants `Enemy` et `Health` sur chaque instance si l'on veut faire apparaître 10 000 ennemis simultanément ?
- **La réponse courte :** Non. Bien que mon architecture (Object Pool + Proxy + Flyweight) soit l'approche **GameObject (MonoBehaviour)** la plus optimisée possible (Slide 50 : "Stable, éprouvé"), elle possède des limites.
- **La réponse longue (Data Locality) :** Chaque `MonoBehaviour` et chaque `GameObject` a un surcoût en mémoire (Heap) et en temps CPU (appels virtuels, gestion de la hiérarchie). Pour atteindre une performance extrême (milliers d'entités), il faut repenser l'architecture selon l'**Entity Component System (ECS)** via Unity DOTS. Au lieu d'avoir un composant `Health` sur chaque ennemi, j'aurais un immense tableau contigu en mémoire contenant uniquement des entiers (`HP`), et un `System` unique chargé de parcourir ce tableau de façon ultra-rapide (Data Locality).
- **Mon choix :** Comme indiqué Slide 51, *"On peut tout à fait mélanger les deux approches"*. Pour un prototype fonctionnel et facile à maintenir, l'approche *GameObject optimisée* est idéale. Si le jeu devenait un projet commercial de très grande envergure, il faudrait basculer ces systèmes vitaux (déplacement, vie) sous DOTS.
