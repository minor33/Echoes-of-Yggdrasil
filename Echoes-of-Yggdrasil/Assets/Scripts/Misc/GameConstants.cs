using UnityEngine;


public static class GameConstants {
    public const bool DEBUG = true;
}

public enum Target {
    CHOOSE,
    RANDOM,
    FRONT,
    SELF
}

public enum Keyword {
    DAMAGE,
    BLOCK,
    TARGET,
    DRAW,
    DUPLICATE,
    INVOKE,
    STABLE,
    NULL
}

public enum EnemyAction {
    ATTACK,
    DEFEND,
    PAUSE
}
