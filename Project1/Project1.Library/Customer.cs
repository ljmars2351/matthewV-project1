﻿using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;

namespace Project1.Library
{
    public class Customer : IDClass
    {
        private string _firstname;
        private string _lastname;
        public string Email { get; set; } //Nullable.
        public Address Address { get; set; } //Nullable. Value if customer wants to set a default location to deliver order to.
        private Store _store; //customer has a default pizza store

        public string FirstName
        {
            get => _firstname;
            set
            {
                Guard.Against.NullOrWhiteSpace(value, nameof(value));
                _firstname = value;

            }
        }

        public string LastName
        {
            get => _lastname;
            set
            {
                Guard.Against.NullOrWhiteSpace(value, nameof(value));
                _lastname = value;
            }
        }

        public Store Store
        {
            get => _store;
            set
            {
                _store = value ?? throw new ArgumentNullException(nameof(value), "Customer's store must not be null.");
            }
        }

        public Customer() { }

        public Customer(string firstName, string lastName, string email)
        {
            Guard.Against.NullOrWhiteSpace(firstName, nameof(firstName));
            Guard.Against.NullOrWhiteSpace(lastName, nameof(lastName));

            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }


        public ICollection<Order> Orders { get; set; } //the orders by this customer. Reverse navigation property

    }

}
