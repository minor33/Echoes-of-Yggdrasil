using UnityEngine;


public static class GameConstants {
    public const bool DEBUG = true; // For nice debugs that don't make the console unreadable (but maybe print very often)
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
    CALM_DOWN,  // Intended for rage abilities, will probably give all damage - damage until the next rage queue goes off if in a play effect.
    SKIP,       // Intended for rage abilities, will probably skip the first X cards triggered in the rage queue if in a play effect.
    RETAIN,     // Intended for rage abilities, will probably do nothing in play abilities
    TACTICAL,   // Intended for play abillites, will probably do nothing in rage abilities
    SWAP,       // Intended for play abilities, FOR THE LOVE OF GOD DON'T USE THIS IN A RAGE ABILITY EVERYTHING WILL EXPLODE (probably)
    STARTER,    // Intended for rage abiliites, will probably do nothing in play abilities
    FINISHER,   // Intended for rage abilities, will probably do nothing in play abilities
    EXPAND,     // Intended for play abilities, will appear to do nothing in the rage ability and then immediately lose the expand
    REMOVE,     // Intended for play abilities, FOR THE LOVE OF GOD DON'T USE THIS IN A RAGE ABILITY EVERYTHING WILL EXPLODE (probably)
    REVERSE,    // Intended for play abilities, FOR THE LOVE OF GOD DON'T USE THIS IN A RAGE ABILITY EVERYTHING WILL EXPLODE (probably)
    REPEAT,     // Intended for rage abiliites, will probably do nothing in play abilities (but probably should?)
    PATIENT,    // Intended for rage abiliites, will probably do nothing in play abilities
    SET_EVOKE,  // Should work in both ability types, DO NOT: Set Evoke to an Evoke ability, or set evoke to a choose ability. Light error handling in the form of description edits is included. 
    EVOKE,      // Should work in both ability types
}

public enum EnemyAction {
    ATTACK,
    DEFEND,
    PAUSE
}
