using System;

public class AttendanceModel
{
    public int AttendanceID { get; set; }
    public int Emp_ID { get; set; }
    public DateTime AttendanceDate { get; set; }

    public string FirstHalfStatus { get; set; } 
    public string SecondHalfStatus { get; set; } 

    public DateTime? LoginTime { get; set; }
    public DateTime? LogoutTime { get; set; }

    public int? ModeID { get; set; } 
    public DateTime CreatedOn { get; set; }
    public DateTime Date { get; set; } 
    public string Status { get; set; }
    public string FullStatus { get; set; }
    public string AttendanceMode { get; set; } 
    public string SelectedHalf { get; set; } 

    public string HolidayName { get; set; }
    public DateTime HolidayDate { get; set; }

}

