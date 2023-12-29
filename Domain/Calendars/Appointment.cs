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

namespace Domain.Calendars;

public class Appointment : Entity<Guid?>
{
    public string Title { get; private set;} = "";
    public string Description { get; private set;} = "";
    public DateTime DueDate { get; private set;} = DateTime.Now;
    public TimeSpan TimeRange { get; private set;}
    public List<int> ReminderInMinutes {get; private set;} = [];
    public bool Done {get; private set;} = false;

    public Appointment() : base(null)
    {
    }

    public Appointment(string title, string description, DateTime dueDate, TimeSpan timeRange): base(null)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        TimeRange = timeRange;

        this.AddDomainEvent(new AppointmentCreatedEvent(this));
    }

    public void ChangeTitle(string title)
    {
        if (string.IsNullOrEmpty(Title))
            return;

        Title = title;
        this.AddDomainEvent(new AppointmentTitleChangedEvent(this));
    }

    public void ChangeDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
            return;
        
        Description = description;
        this.AddDomainEvent(new AppointmentDescriptionChangedEvent(this));
    }

    public void ChangeDate(DateTime dueDate)
    {
        DueDate = dueDate;
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
            this.AddDomainEvent(new AppointmentReminderAddedEvent(this));
        }
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
        foreach (int reminder in ReminderInMinutes)
        {
            return 
                (DueDate.Subtract(timeValue).Minutes > 0) && 
                (DueDate.Subtract(timeValue).Minutes <= reminder);
        }

        return false;
    }
}