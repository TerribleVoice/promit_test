namespace promit_test
{
    public class Word 
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int Amount { get; set; }

        public Word(string word)
        {
            Value = word;
        }
        
        public Word(string word, int amount)
        {
            Value = word;
            Amount = amount;
        }

        public Word() { }
            
        public override bool Equals(object obj)
        {
            if (!(obj is Word))
                return false;

            var word = (Word)obj;

            return Value.Equals(word.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
        
    }
}