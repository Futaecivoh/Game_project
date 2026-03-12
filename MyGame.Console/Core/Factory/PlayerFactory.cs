class PlayerFactory : CreatureFactory
{
    public override Creature CreateCreature(string name)
    {
        return new Player { Name = name };
    }
}