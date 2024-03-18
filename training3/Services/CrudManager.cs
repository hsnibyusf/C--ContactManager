using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using training3.ManagerContext;
using training3.Models;

public class CrudManager
{
    private readonly dbContext dbContext;

    public CrudManager(dbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public void AddEntity<TEntity>(TEntity entity) where TEntity : class
    {
        dbContext.Set<TEntity>().Add(entity);
        dbContext.SaveChanges();
    }
    public List<TEntity> LoadEntities<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryModifier = null) where TEntity : class
    {
        IQueryable<TEntity> query = dbContext.Set<TEntity>();

        if (queryModifier != null)
        {
            query = queryModifier(query);
        }

        return query.ToList();
    }
    public void AddAddress(Address address)
    {
        AddEntity(address);
    }
    public List<Address> GetAllAddresses(int contactId)
    {
        return LoadEntities<Address>(query => query.Where(a => a.ContactId == contactId));
    }
    public void AddEmail(Email email)
    {
        AddEntity(email);
    }
    public List<Email> GetAllEmails(int contactId)
    {
        return LoadEntities<Email>(query => query.Where(e => e.ContactId == contactId));
    }
    public void AddPhone(Phone phone)
    {
        AddEntity(phone);
    }
    public List<Phone> GetPhonesByContactId(int contactId)
    {
        return LoadEntities<Phone>(query => query.Where(p => p.ContactId == contactId));
    }
    public void AddContact(Contact contact)
    {
        AddEntity(contact);
    }
    public List<Contact> GetAllContacts()
    {
        return LoadEntities<Contact>();
    }
    public Contact GetContactById(int contactId)
    {
        return dbContext.Contacts.FirstOrDefault(c => c.Id == contactId);
    }
    public void EditContact(int contactId, Contact updatedContact)
    {
        Contact contact = dbContext.Contacts.Find(contactId);
        if (contact != null)
        {
            contact.FullName = updatedContact.FullName;
            contact.DateOfBirth = updatedContact.DateOfBirth;
            contact.Gender = updatedContact.Gender;

            dbContext.SaveChanges();
        }
    }
    public void DeleteContact(int contactId)
    {
        Contact contact = dbContext.Contacts.Find(contactId);
        if (contact != null)
        {
            dbContext.Contacts.Remove(contact);
            dbContext.SaveChanges();
        }
    }
    public void DeleteEmail(int emailId)
    {
        Email email = dbContext.Emails.Find(emailId);
        if (email != null)
        {
            dbContext.Emails.Remove(email);
            dbContext.SaveChanges();
        }
    }
    public void DeletePhone (int phoneId)
    {
        Phone phone = dbContext.Phones.Find(phoneId);
        if (phone != null)
        {
            dbContext.Phones.Remove(phone);
            dbContext.SaveChanges();
        }
    }
    public void AddressDelete(int addressId)
    {
        Address address = dbContext.Addresses.Find(addressId);
        if (address != null)
        {
            dbContext.Addresses.Remove(address);
            dbContext.SaveChanges();
        }

    }
    public List<Contact> SearchContactsByName(string searchTerm)
    {
        IQueryable<Contact> query = dbContext.Contacts;

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(c => c.FullName.Contains(searchTerm));
        }

        return query.ToList();
    }
    public List<Contact> SearchContactsByDates(DateTime? startDate, DateTime? endDate)
    {
        IQueryable<Contact> query = dbContext.Contacts;

        if (startDate.HasValue && endDate.HasValue)
        {
            query = query.Where(c => c.DateOfBirth >= startDate && c.DateOfBirth <= endDate);
        }
        else if (startDate.HasValue)
        {
            query = query.Where(c => c.DateOfBirth >= startDate);
        }
        else if (endDate.HasValue)
        {
            query = query.Where(c => c.DateOfBirth <= endDate);
        }

        return query.ToList();
    }

   
}
