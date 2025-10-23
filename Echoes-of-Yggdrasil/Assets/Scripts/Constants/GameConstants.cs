using UnityEngine;

public static class GameConstants {
    public const int CHOOSE = 0;
    public const int RANDOM = 1;
    public const int FRONT = 2;
    public const int SELF = 3;
}

public enum Keyword {
    DAMAGE,
    BLOCK,
    TARGET,
    DRAW
}

public enum EnemyAction {
        ATTACK,
        BLOCK,
        NEG_STATUS,
        POS_STATUS,
        PAUSE
}
