using System;

public class AttendanceModel
{
    public int AttendanceID { get; set; }
    public int Emp_ID { get; set; }
    public DateTime AttendanceDate { get; set; }

    public string FirstHalfStatus { get; set; } // "Present" / "Absent"
    public string SecondHalfStatus { get; set; } // "Present" / "Absent"

    public int ModeID { get; set; } // WFO/WFH
    public DateTime CreatedOn { get; set; }

    // Extra fields for dropdown display
    public string AttendanceMode { get; set; } // Mode table se aayega
    public string SelectedHalf { get; set; } // For binding from form: "First Half", etc.
}

