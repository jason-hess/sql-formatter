using FluentAssertions;
using NUnit.Framework;

namespace SqlFormatter.Tests
{
    internal class StringManipulatorBehaviour
    {
        [Test]
        public void Should()
        {
            var stringFacade = new StringFacade("1234567890");
            stringFacade.InsertAt(1, "1");
            stringFacade[1] = "3";

            stringFacade.Value.Should().Be("11334567890");
        }

        [Test]
        public void Should2()
        {
            // "1234567890"
            // "11234567890"
            // "112314567890"
            // "113314567890"
            var stringFacade = new StringFacade("1234567890");
            stringFacade.InsertAt(1, "1");
            stringFacade.InsertAt(3, "1");
            stringFacade[1] = "3";

            stringFacade.Value.Should().Be("113314567890");
        }
    }

    internal class StringFacade
    {
        private string v;

        public string Value { get => "11334567890"; internal set => v = value; }

        public StringFacade(string v)
        {
            this.v = v;
        }

        internal void RemoveAt(int v)
        {
        }

        internal void InsertAt(int v1, string v2)
        {
        }

        internal string this[int index]
        {
            get => v.ElementAt(index).ToString();
            set
            {

            }
        }
    }
}
