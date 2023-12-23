using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Shared;
using System.Globalization;

namespace Domain.Calendar;

public class Calendar : AggregateRoot<Guid?>
{
    public CultureInfo ActiveCulture {get; private set;}
    public int ActiveWeek {get; private set;}
    public List<Appointment> Appointments {get; private set;} = [];

    public Calendar(Guid id, CultureInfo activeCultureInfo, List<Appointment> appointments): base(id)
    {
        ActiveCulture = activeCultureInfo;
        ActiveWeek = ActiveCulture.Calendar.GetWeekOfYear(
            DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday
        );
        Appointments = appointments;
    }

    public Calendar(CultureInfo activeCultureInfo, List<Appointment> appointments): base(null)
    {
        ActiveCulture = activeCultureInfo;
        ActiveWeek = ActiveCulture.Calendar.GetWeekOfYear(
            DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday
        );
        Appointments = appointments;
    }

    public void MakeNewAppointment(Appointment[] appointments)
    {
        foreach (var appointment in appointments)
            Appointments.Add(appointment);
    }

    public List<Appointment> CheckActiveAppointments(DateTime dayValue)
    {
        return Appointments.Where(a => a.IsInReminderRange(dayValue)).ToList();
    }
}