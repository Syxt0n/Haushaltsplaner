using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Shared;
using System.Globalization;

namespace Domain.Calendar;

public static class Calendar
{
    public static CultureInfo ActiveCulture {get; private set;}
    public static int ActiveWeek {get; private set;}
    public static List<Appointment> Appointments {get; private set;} = [];

    public static void SetupCalendar(CultureInfo activeCultureInfo)
    {
        ActiveCulture = activeCultureInfo;
        ActiveWeek = ActiveCulture.Calendar.GetWeekOfYear(
            DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday
        );
    }

    public static void MakeNewAppointment(Appointment[] appointments)
    {
        foreach (var appointment in appointments)
            Appointments.Add(appointment);
    }

    public static List<Appointment> CheckActiveAppointments(DateTime dateTime)
    {
        return Appointments.Where(a => a.IsInReminderRange(dateTime)).ToList();
    }
}