using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO; // קישור חשוב
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace http.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private const string FilePath = "students.json"; // מיקום הקובץ
        private static List<string> students;

        static StudentController()
        {
            LoadStudents(); // טוען את הסטודנטים מהקובץ
        }

        // טוען את הסטודנטים מהקובץ
        private static void LoadStudents()
        {
            if (System.IO.File.Exists(FilePath)) // ודא ש-File נמצא
            {
                var json = System.IO.File.ReadAllText(FilePath);
                students = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();
            }
            else
            {
                // אם הקובץ לא קיים, אתחיל עם רשימה ברירת מחדל
                students = new List<string>
                {
                    "Leah", "Rachel", "Shifra", "Noa", "Miryam"
                };
            }
        }

        // שומר את הסטודנטים לקובץ
        private static void SaveStudents()
        {
            var json = JsonConvert.SerializeObject(students, Formatting.Indented);
            System.IO.File.WriteAllText(FilePath, json); // שומר את הנתונים לקובץ
        }

        // GET: api/student
        [HttpGet]
        public ActionResult<List<string>> GetStudents()
        {
            return Ok(students); // מחזיר את הרשימה סטטוס 200  
        }
        [HttpGet("id")]
        public ActionResult<string> GetStudentById(int id)
        {
            if(id<students.Count()) 
                return Ok(students[id]); // מחזיר את הרשימה סטטוס 200
            return NotFound("not found");//סטטוס 404 

        }

        // POST: api/student
        [HttpPost]
        public ActionResult AddStudent([FromBody] string student)
        {
            if (string.IsNullOrWhiteSpace(student))
            {
                return BadRequest("Student name cannot be empty.");//סטטוס:400 לא התקבל קלט
            }

            students.Add(student); // מוסיף את הסטודנט לרשימה
            SaveStudents(); // שומר את הרשימה בקובץ
            return Created(FilePath, student);
        }    //  סטטוס 201 נוצר חדש

        [HttpDelete]
        public ActionResult delete(int index)
        {
            if (students.Count>index)
            {
                students.Remove(students[index]);
                SaveStudents();
                return Ok(students);
            }
            return NotFound("not found"); // לא נמצא, סטטוס: 404

        }

    }
}
