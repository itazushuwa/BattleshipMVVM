using System;

namespace Battleship_2.Models.FieldComponents
{
    public partial class Cell
    {
        public int I { get; set; }
        public int J { get; set; }

        public Cell() : this(0, 0) { }
        public Cell(int i, int j)
        {
            I = i;
            J = j;
        }
    }



    public partial class Cell
    {
        private static readonly Random _random = new Random();

        public static Cell NotValid => new Cell(-1, -1);
        public static Cell RandomCell =>  new Cell(_random.Next(Field.FieldRows), _random.Next(Field.FieldColumns));
    }
}
