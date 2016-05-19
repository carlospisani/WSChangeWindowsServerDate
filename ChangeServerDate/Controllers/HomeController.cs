using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

public class PlatformInvoker
{
    [DllImport("msvcrt.dll")]
    public static extern int puts(string c);
    [DllImport("msvcrt.dll")]
    internal static extern int _flushall();

    public static void Main()
    {
        puts("Test");
        _flushall();
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
        public short wYear;
        public short wMonth;
        public short wDayOfWeek;
        public short wDay;
        public short wHour;
        public short wMinute;
        public short wSecond;
        public short wMilliseconds;
    }

    /// Return Type: BOOL->int
    ///lpSystemTime: SYSTEMTIME*
    [DllImport("kernel32.dll", EntryPoint = "SetSystemTime")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetSystemTime([InAttribute()] ref SYSTEMTIME lpSystemTime);
}

namespace ChangeServerDate.Controllers
{
    public class FakeDate
    {
        public short Year { get; set; }
        public short Month { get; set; }
        public short Day { get; set; }
        public short Hour { get; set; }
        public short Minute { get; set; }
        public short Second { get; set; }
    }

    public class HomeController : Controller
    {
        [HttpPost]
        public bool Index(FakeDate fakeDate)
        {
            var st = new PlatformInvoker.SYSTEMTIME();
            st.wYear = fakeDate.Year; 
            st.wMonth = fakeDate.Month;
            st.wDay = fakeDate.Day;
            st.wHour = (short)(fakeDate.Hour+3);
            st.wMinute = fakeDate.Minute;
            st.wSecond = fakeDate.Second;
            PlatformInvoker.SetSystemTime(ref st); 

            return true;
        }
    }
}