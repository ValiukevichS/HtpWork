using System;
using System.Collections.Generic;
using System.Text;

namespace HomeWork
{
    public class CaramelCandy : Candy
    {
        public CaramelCandy()
            : base(100, 200) { }
        protected override string NameCandy => "карамельная конфета";
    }

    public class ChocolateCandy : Candy
    {
        public ChocolateCandy()
            : base(10, 70) { }
        protected override string NameCandy => "Конфета Шоколадная";
    }

    public class HotCandy : Candy
    {
        public HotCandy()
            : base(50, 150) { }

        protected override string NameCandy => "Конфета из горького шеколада";
    }

    public class JellyCandy : Candy
    {
        public JellyCandy()
            : base(200, 220) { }

        protected override string NameCandy => "Желотиновая конфета";
    }

    public class MadeInTurkyCandy : Candy
    {
        public MadeInTurkyCandy()
            : base(300, 300) { }

        protected override string NameCandy => "Турецкая сладость";
    }
}
