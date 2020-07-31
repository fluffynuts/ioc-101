using NUnit.Framework;
using static PeanutButter.RandomGenerators.RandomValueGen;
using NExpect;
using static NExpect.Expectations;

namespace ioc_101.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void ShouldBeAbleToRegisterAndCreateOnThing()
        {
            // Arrange
            var container = Create();
            // Act
            container.Register<IMooCow, MooCow>();
            var moocow = container.Resolve<IMooCow>();
            // Assert
            Expect(moocow)
                .To.Be.An.Instance.Of<MooCow>();
        }

        [Test]
        public void ShouldBeAbleToConstructAThingWithDependencies()
        {
            // Arrange
            var container = Create();
            container.Register<IKennel, Kennel>();
            container.Register<IDoggo, Doggo>();
            container.Register<IDogFarm, DogFarm>();
            // Act
            var kennel = container.Resolve<IDogFarm>();
            // Assert
            Expect(kennel)
                .To.Be.An.Instance.Of<DogFarm>();
        }

        public interface IDogFarm
        {
        }

        public class DogFarm: IDogFarm
        {
            public IKennel Kennel { get; }

            public DogFarm(IKennel kennel)
            {
                Kennel = kennel;
            }
        }

        public interface IKennel
        {
            IDoggo Doggo { get; }
        }

        public class Kennel
            : IKennel
        {
            public IDoggo Doggo { get; }

            public Kennel(IDoggo doggo)
            {
                Doggo = doggo;
            }
        }

        private IContainer Create()
        {
            return new Container();
        }
    }
}