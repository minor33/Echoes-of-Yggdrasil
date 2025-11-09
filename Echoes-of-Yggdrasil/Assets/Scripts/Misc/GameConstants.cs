using UnityEngine;


public static class GameConstants {
    public const bool DEBUG = true; // For nice debugs that don't make that console unreadable
    public const bool XCESSIVEDEBUG = false; // For debugs that print every frame cause fuck those
}

public enum Target {
    CHOOSE,
    RANDOM,
    FRONT,
    SELF
}

public enum Keyword {
    DAMAGE,     // Do damage to target
    BLOCK,      // Add block to self. Currently not implemented with TARGET keyword. 
    TARGET,     // Intended for use with other keywords which have targets, mainly damage and block (not implemented). 
                // Use before the keyword. Can be used multiple times for different targets. Should default to front enemy in all cases. 
    DRAW,       // Card draw
    DUPLICATE,  // Intended for play abilities, will probably add duplicate to the the next card played if used in a rage effect.
    INVOKE,     // Intended for rage abilities, will probably add invoke to the first card tirggered in the rage queue if in a play effect. 
    STABLE,     // Intended for rage abilities, will not work outside of them. 
    NULL,       // Empty keyword for misc. use
    ANGRIER,    // Intended for rage abilities, will probably give all damage + damage until the next rage queue goes off if in a play effect.
}

public enum EnemyAction {
    ATTACK,
    DEFEND,
    PAUSE
}
