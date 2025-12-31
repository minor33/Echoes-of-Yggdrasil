using UnityEngine;
using System.Collections.Generic;


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
    RECALL,     // Should work in both abillity types
}

public static class KeywordDescriptions {
    // At some point more information may need to be gathered from this (example: should the description be displayed) but this works for now
    public static readonly Dictionary<Keyword, string> Descriptions = new Dictionary<Keyword, string>() {
        // Helpers for writing descriptions (as to the terminology I'm using)
        // Card: The actual card itself. Should be used when dealing with big picture stuff such as removing and playing the card without any care as to what it does
        // Effect: The effect of the card. Should be used when triggering the ability of the card or referencing its ability. Do not use the word ability in place of effect
        // Trigger: One single proc of an effect on a card. Includes all forms of repeat, such as REPEAT, PATIENT, STARTER, INVOKE, and more. 
        { Keyword.DAMAGE, "Does X damage to the target." },
        { Keyword.BLOCK, "Adds X block to the target" }, // DOES NOT WORK WITH TARGET
        { Keyword.TARGET, "Specify target for next keywords. Defaults to front enemy." }, 
        { Keyword.DRAW, "Draw X cards from your deck" },
        { Keyword.DUPLICATE, "Add X additional copies of this card to the rage queue when played" },
        { Keyword.INVOKE, "Trigger the next effect in the rage queue X additional times" },
        { Keyword.STABLE, "This card can not be pushed off the rage queue when overflowing" },
        { Keyword.NULL, "No effect, this should never be seen anywhere in game" },
        { Keyword.ANGRIER, "All damage effects deal X additional damage in the current rage queue" },
        { Keyword.CALM_DOWN, "All damage effects deal X less damage in the current rage queue" },
        { Keyword.SKIP, "The next X cards in the rage queue trigger one less time" },
        { Keyword.RETAIN, "This card stays in the rage queue X additional times" }, // Still not in love with this wording
        { Keyword.TACTICAL, "This card does not get added to the rage queue when played" },
        { Keyword.SWAP, "Swap 2 cards in the rage queue X times" },
        { Keyword.STARTER, "This effect triggers an additional X times if it is the first card in the rage queue" },
        { Keyword.FINISHER, "This effect triggers an additional X times if it is the last card in the rage queue" },
        { Keyword.EXPAND, "Increase the size of the rage queue by X until the next time it is triggered" },
        { Keyword.REMOVE, "Remove X cards in the rage queue" },
        { Keyword.REVERSE, "Reverse the order of the rage queue" }, // Shouldn't be displayed because full text is written in description
        { Keyword.REPEAT, "This effect gets triggered an additional X times" }, 
        { Keyword.PATIENT, "This effect get triggered an additional number of times equal to the number of triggers that have already occured in the rage queue" },
        { Keyword.SET_EVOKE, "Sets the effect of Evoke to the listed effect" },
        { Keyword.EVOKE, "Triggers the set Evoke effect X times" },
        { Keyword.RECALL, "Triggers the rage effect of the top card of your discard pile X times. Ignores Recall on triggered card."}
    };
    
    // Helper method to get a description
    public static string GetDescription(Keyword keyword) {
        if (Descriptions.TryGetValue(keyword, out string description)) {
            return description;
        }
        return "No description available";
    }
}

public enum EnemyAction {
    ATTACK,
    DEFEND,
    PAUSE
}
