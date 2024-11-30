namespace Entities.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message)
            : base(message)
        { }
    }

    public sealed class ContactsNotFoundException : NotFoundException
    {
        public ContactsNotFoundException() 
            : base("Contact doesn't exist!")
        {
            
        }

        public ContactsNotFoundException(string filter)
            : base($"Contacts for '{filter}' doesn't exist!")
        {
            
        }
    }

    public sealed class CompaniesNotFoundException : NotFoundException
    {
        public CompaniesNotFoundException(string filter)
            : base($"Companies for '{filter}' doesn't exist!")
        {
            
        }
    }

    public sealed class DealsNotFoundException : NotFoundException
    {
        public DealsNotFoundException(string filter)
            : base($"Deals for'{filter}' doesn't exist!")
        {
            
        }
    }

    public sealed class LeadsNotFoundException : NotFoundException
    {
        public LeadsNotFoundException(string filter)
            : base($"Leads for {filter} doesn't exist!")
        {
            
        }
    }
}
