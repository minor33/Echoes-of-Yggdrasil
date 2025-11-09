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
    // Intended for Play Only (may have catasprophic or weird effects if used in the other)
    DUPLICATE,
    // Intended for Rage Only (may have catasprophic or weird effects if used in the other)
    INVOKE,
    // Blank Keyword
    NULL
}

public enum EnemyAction {
    ATTACK,
    DEFEND,
    PAUSE
}
