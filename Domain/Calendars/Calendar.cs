using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Shared;
using System.Globalization;

namespace Domain.Calendars;

public class PersonalCalendar : AggregateRoot<Guid?>
{
    public CultureInfo ActiveCulture {get; private set;}
    public int ActiveWeek {get; private set;}
    public List<Appointment> Appointments {get; private set;} = [];

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public PersonalCalendar(): base(null)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {        
    }

    public PersonalCalendar(CultureInfo activeCultureInfo, List<Appointment> appointments): base(null)
    {
        ActiveCulture = activeCultureInfo;
        ActiveWeek = ActiveCulture.Calendar.GetWeekOfYear(
            DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday
        );
        Appointments = appointments;

        Validate();
        this.AddDomainEvent(new CalendarCreatedEvent(this));
    }

    public void MakeNewAppointment(Appointment[] appointments)
    {
        List<Appointment> result = [];

        foreach (var appointment in appointments)
        {
            if (!Appointments.Contains(appointment))
            {
                Appointments.Add(appointment);
                result.Add(appointment);
            }
        }

        Validate();
        this.AddDomainEvent(new CalendarAppointmentAddedEvent(this, result));
    }

    public List<Appointment> CheckActiveAppointments(DateTime dayValue)
    {
        return Appointments.Where(a => a.IsInReminderRange(dayValue)).ToList();
    }

    public override void Validate()
    {
        if (ActiveWeek > 53 || ActiveWeek < 1)
            throw new ArgumentNullException("ActiveWeek", "Calender must have a valid Weeknumber.");

        foreach (var appointment in Appointments)
            appointment.Validate();
    }
}