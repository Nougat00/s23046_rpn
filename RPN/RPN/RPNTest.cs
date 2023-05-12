using NUnit.Framework;

namespace RPN
{
    [TestFixture]
    public class TestRPN
    {
        private Rpn _rpn;

        [SetUp]
        public void Setup()
        {
            _rpn = new Rpn();
        }

        [Test]
        public void EvalRpnShouldReturnMinus23()
        {
            Assert.AreEqual(-23, _rpn.EvalRpn("12 3 9 15 18 2 - - * + /"));
        }

        [Test]
        public void EvalRpnShouldReturn120()
        {
            Assert.AreEqual(120, _rpn.EvalRpn("5 !"));
        }

        [Test]
        public void EvalRpnShouldReturn1()
        {
            Assert.AreEqual(1, _rpn.EvalRpn("-1 |"));
        }

        [Test]
        public void EvalRpnShouldReturn1120()
        {
            Assert.AreEqual(1120, _rpn.EvalRpn("#A0 B11111 D4 ! - | *"));
        }


        [Test]
        public void EvalRpnShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _rpn.EvalRpn("0 8 /"));
        }


        [Test]
        public void EvalRpnShouldThrowFormatException()
        {
            Assert.Throws<FormatException>(() => _rpn.EvalRpn("DB #G0 B223 + +"));
        }

        [Test]
        public void EvalRpnShouldThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _rpn.EvalRpn("8 9"));
        }
    }
}