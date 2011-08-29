

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _AMTrail
    {

        [Test]
        public void Initialize()
        {
            AMTrail trail = AMTrail.Instance;

            trail.Initialize();

            Assert.AreEqual(0, trail.TR);
        }

        [Test]
        public void Stop()
        {
            AMTrail trail = AMTrail.Instance;

            Assert.IsTrue(trail.Stop());
        }

        [Test]
        public void Trail()
        {
            AMTrail trail = AMTrail.Instance;

            AbstractTerm term = new AbstractTerm();
            trail.Initialize();
            trail.Trail(term);

            Assert.AreEqual(1, trail.TR);
        }
    }
}