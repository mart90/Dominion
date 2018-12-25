namespace Dominion
{
    public enum CardType
    {
        Default,
        Victory,
        Action,
        Attack,
        Reaction,
        Treasure,
        Curse
    }

    public enum Expansion
    {
        Default,
        Base
    }

    public enum EffectPhase
    {
        Default,
        OnPlay,
        OnGain,
        OnTrash,
        OnReaction
    }
}
