using System;
using System.Collections.Generic;
using System.Linq;

namespace ioc_101
{
    class Program
    {
        static void Main(string[] args)
        {
            var bootstrapper = new Bootstrapper();;
            var container = bootstrapper.Bootstrap();
            var farm = container.Resolve<IFarm>();
            farm.PokeAllAnimals();
        }
    }

    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var container = new Container();
            container.Register<IDoggo, Doggo>();
            container.Register<IDoggo, Bernie>();
            container.Register<IMooCow, MooCow>();
            container.Register<IWriter, ConsoleWriter>();
            container.Register<IFarm, Farm>(); // <-
            
            return container;
        }
    }

    public interface IContainer
    {
        T Resolve<T>();
        void Register<TService, TImplementation>() where TImplementation : TService;
    }

    public class Container
        : IContainer
    {
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type t)
        {
            if (!_registrations.TryGetValue(t, out var concrete))
            {
                throw new Exception("Whoops");
            }
            
            var constructors = concrete.GetConstructors();
            var ctor = constructors.First();
            var parameters = ctor.GetParameters();
            if (parameters.Length == 0)
            {
                return Activator.CreateInstance(concrete);
            }

            var dependencies = parameters.Select(p =>
            {
                var t = p.ParameterType;
                return Resolve(t);
            });
            return Activator.CreateInstance(concrete, dependencies.ToArray());
        }

        private readonly Dictionary<Type, Type> _registrations
            = new Dictionary<Type, Type>();

        public void Register<TService, TImplementation>()
            where TImplementation : TService
        {
            _registrations[typeof(TService)] = typeof(TImplementation);
        }
    }

    public interface IAnimalFactory
    {
        IDoggo CreateDoggo();
        IMooCow CreateMooCow();
    }

    public class AnimalFactory
        : IAnimalFactory
    {
        public IDoggo CreateDoggo()
        {
            return new Doggo();
        }

        public IMooCow CreateMooCow()
        {
            return new MooCow();
        }
    }

    public interface IFarm
    {
        void PokeAllAnimals();
    }

    public class Farm : IFarm
    {
        private readonly IDoggo _doggo;
        private readonly IMooCow _mooCow;
        private readonly IWriter _writer;

        public Farm(IDoggo doggo, IMooCow mooCow, IWriter writer)
        {
            _doggo = doggo;
            _mooCow = mooCow;
            _writer = writer;
        }

        // lots of lines of code go here

        public void PokeAllAnimals()
        {
            _writer.WriteLine($"Doggo says:  {_doggo.MakeNoise()}");
            _writer.WriteLine($"MooCow says: {_mooCow.MakeNoise()}");
        }
    }

    public interface IAnimal
    {
        string MakeNoise();
    }

    public interface IMooCow : IAnimal
    {
    }

    public class MooCow : IMooCow
    {
        public string MakeNoise()
        {
            return "moo";
        }
    }

    public interface IDoggo : IAnimal
    {
    }

    public class Bernie : IDoggo
    {
        public string MakeNoise()
        {
            return "le woof";
        }
    }

    public class Doggo : IDoggo
    {
        public string MakeNoise()
        {
            return "woof";
        }
    }
}