using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace TellDontAsk
{
    public class DataSource
    {
        private readonly IDatabase _database;

        public DataSource(IDatabase database)
        {
            _database = database;
        }

        public void Person(int id, Action<Person> actionOnPerson)
        {
            foreach (var p in _database.GetRows<Person>(id))
                actionOnPerson(p);
        }

        public void UpdatePerson(Person person)
        {
            _database.Update(person);
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
    }

    [TestFixture]
    public class DataSourceTests
    {
        private IDatabase _database;
        private DataSource _dataSource;

        [SetUp]
        public void Setup()
        {
            _database = Substitute.For<IDatabase>();
            _dataSource = new DataSource(_database);
        }

        [Test]
        public void Person_object_is_returned_in_action()
        {
            var myPerson = new Person { Id = 42 };
            _database.GetRows<Person>(42).Returns(new List<Person> { myPerson });
            _dataSource.Person(42, person =>
                {
                    person.PhoneNumber = "1234567890";
                    _dataSource.UpdatePerson(person);
                });
            _database.Received().Update(myPerson);
        }
    }
}
