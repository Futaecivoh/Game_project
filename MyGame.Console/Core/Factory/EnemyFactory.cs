class EnemyFactory : CreatureFactory
{
    public override Creature CreateCreature(string name)
    {
        return new Enemy { Name = name };
    }
}