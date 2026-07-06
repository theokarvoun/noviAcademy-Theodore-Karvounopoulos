namespace WorldRank
{
    public class Player
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public int Score { get; private set; }
        public Player(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
            }
            Id = Guid.NewGuid();
            this.Name = name;
            Score = 0;
        }
        public void AddScore(int points)
        {
            if (points < 0)
            {
                throw new ArgumentException("Points cannot be negative.", nameof(points));
            }
            Score += points;
        }
        public override string ToString()
        {
            return $"Player: {Name}, Score: {Score}";
        }
    }
}