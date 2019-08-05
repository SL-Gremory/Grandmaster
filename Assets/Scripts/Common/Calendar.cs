using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calendar : MonoBehaviour
{

    public static Calendar Instance { get; private set; }

    public static DateTime currentDate = new DateTime(2001, 12, 1);

    public static Zodiac currentZodiac;



    void Awake()
    {
        Instance = this;
        currentZodiac = GetZodiacByDate(currentDate);
    }


    public void NextDay()
    {
        currentDate = currentDate.AddDays(1);
        currentZodiac = GetZodiacByDate(currentDate);
    }

    public static Zodiac GetZodiacByDate(DateTime date)
    {
        int currentYear = date.Year;
       
        if(DateIsBetween(date, new DateTime(currentYear, 3, 21), new DateTime(currentYear, 4, 20)))
        {
            return Zodiac.ARIES;
        }
        else if(DateIsBetween(date, new DateTime(currentYear, 4, 21), new DateTime(currentYear, 5, 21)))
        {
            return Zodiac.TAURUS;
        }
        else if(DateIsBetween(currentDate, new DateTime(currentYear, 5, 22), new DateTime(currentYear, 6, 21)))
        {
            return Zodiac.GEMINI;
        }
        else if(DateIsBetween(currentDate, new DateTime(currentYear, 6, 22), new DateTime(currentYear, 7, 22)))
        {
            return Zodiac.CANCER;
        }
        else if(DateIsBetween(currentDate, new DateTime(currentYear, 7, 23), new DateTime(currentYear, 8, 22)))
        {
            return Zodiac.LEO;
        }
        else if(DateIsBetween(currentDate, new DateTime(currentYear, 8, 23), new DateTime(currentYear, 9, 23)))
        {
            return Zodiac.VIRGO;
        }
        else if(DateIsBetween(currentDate, new DateTime(currentYear, 9, 24), new DateTime(currentYear, 10, 23)))
        {
            return Zodiac.LIBRA;
        }
        else if(DateIsBetween(currentDate, new DateTime(currentYear, 10, 24), new DateTime(currentYear, 11, 21)))
        {
            return Zodiac.SCORPIO;
        }
        else if(DateIsBetween(currentDate, new DateTime(currentYear, 11, 22), new DateTime(currentYear, 12, 21)))
        {
            return Zodiac.SAGITTARIUS;
        }
        else if(DateIsBetween(currentDate, new DateTime(currentYear, 12, 22), new DateTime(currentYear + 1, 1, 19)))
        {
            return Zodiac.CAPRICORN;
        }
        else if(DateIsBetween(currentDate, new DateTime(currentYear, 1, 20), new DateTime(currentYear, 2, 18)))
        {
            return Zodiac.AQUARIUS;
        }
        else if(DateIsBetween(currentDate, new DateTime(currentYear, 2, 19), new DateTime(currentYear, 3, 20)))
        {
            return Zodiac.PISCES;
        }
        return Zodiac.none;
    }

    public static bool DateIsBetween(DateTime date, DateTime start, DateTime end)
    {
        return (date >= start && date <= end);
    }

    public override string ToString()
    {
        return currentDate.ToString("dd.MM.yyyy");
    }


    public void Update()
    {


    }
}
