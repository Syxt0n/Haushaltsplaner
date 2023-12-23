using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Shared;
using System.Net.Http.Headers;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace Domain.Calendar;

public class Appointment : Entity<Guid?>
{
    public string Title { get; private set;} = "";
    public string? Description { get; private set;} = "";
    public DateTime Date { get; private set;} = DateTime.Now;
    public TimeSpan TimeRange { get; private set;}
    public List<int> ReminderInMinutes {get; private set;} = [];
    public bool Done {get; private set;} = false;

    public Appointment(Guid? id, string title, string? description, DateTime date, TimeSpan timeRange) : base(id)
    {
        Id = id;
        Title = title;
        Description = description;
        Date = date;
        TimeRange = timeRange;
    }

    public Appointment(string title, string? description, DateTime date, TimeSpan timeRange): base(null)
    {
        Title = title;
        Description = description;
        Date = date;
        TimeRange = timeRange;

        this.AddDomainEvent(new AppointmentCreatedEvent(this));
    }

    public bool ChangeTitle(string title)
    {
        if (string.IsNullOrEmpty(Title))
            return false;

        Title = title;
        this.AddDomainEvent(new AppointmentTitleChangedEvent(this));
        return true;
    }

    public void ChangeDescription(string? description)
    {
        Description = description;
        this.AddDomainEvent(new AppointmentDescriptionChangedEvent(this));
    }

    public void ChangeDate(DateTime date)
    {
        Date = date;
        this.AddDomainEvent(new AppointmentDateChangedEvent(this));
    }

    public void ChangeTimeRange(TimeSpan timeRange)
    {
        TimeRange = timeRange;
        this.AddDomainEvent(new AppointmentTimeRangeChangedEvent(this));
    }

    public void AddReminder(int MinutesBeforeDue)
    {
        if (
            (MinutesBeforeDue > 0) && 
            (ReminderInMinutes.Where(x => x == MinutesBeforeDue).Count() == 0)
           )
        {
            ReminderInMinutes.Add(MinutesBeforeDue);
        }
        this.AddDomainEvent(new AppointmentReminderAddedEvent(this));
    }

    public void RemoveReminder(int MinutesBeforeDue)
    {
        ReminderInMinutes.Remove(ReminderInMinutes.Find(x => x == MinutesBeforeDue));
        this.AddDomainEvent(new AppointmentReminderRemovedEvent(this));

    }
    
    public void FinishAppointment()
    {
        Done = true;
        this.AddDomainEvent(new AppointmentDoneEvent(this));
    }

    public bool IsInReminderRange(DateTime timeValue)
    {
        bool result = false;

        foreach (int reminder in ReminderInMinutes)
        {
            if (
                (Date.Subtract(timeValue).Minutes > 0) && 
                (Date.Subtract(timeValue).Minutes <= reminder)
               )
            {
                return true;
            }
        }

        return result;
    }
}