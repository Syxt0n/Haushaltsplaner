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

        Validate();
        this.AddDomainEvent(new AppointmentCreatedEvent(this));
    }

    public void ChangeTitle(string title)
    {
        Title = title;

        Validate();
        this.AddDomainEvent(new AppointmentTitleChangedEvent(this));
    }

    public void ChangeDescription(string description)
    {       
        Description = description;

        Validate();
        this.AddDomainEvent(new AppointmentDescriptionChangedEvent(this));
    }

    public void ChangeDate(DateTime dueDate)
    {
        DueDate = dueDate;

        Validate();
        this.AddDomainEvent(new AppointmentDateChangedEvent(this));
    }

    public void ChangeTimeRange(TimeSpan timeRange)
    {
        TimeRange = timeRange;

        Validate();
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

            Validate();
            this.AddDomainEvent(new AppointmentReminderAddedEvent(this));
        }
    }

    public void RemoveReminder(int MinutesBeforeDue)
    {
        ReminderInMinutes.Remove(ReminderInMinutes.Find(x => x == MinutesBeforeDue));

        Validate();
        this.AddDomainEvent(new AppointmentReminderRemovedEvent(this));
    }
    
    public void FinishAppointment()
    {
        Done = true;

        Validate();
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

    public override void Validate()
    {
        if (string.IsNullOrEmpty(Title))
            throw new ArgumentNullException("Title", "Appointment must have valid Title.");
        
        if (string.IsNullOrEmpty(Description))
            throw new ArgumentNullException("Description", "Appointment must have valid Description.");

        foreach (var reminder in ReminderInMinutes)
            if (reminder < 0)
                throw new ArgumentNullException("Reminder", "Appointment-Reminder must be at least 1 minute.");
    }
}