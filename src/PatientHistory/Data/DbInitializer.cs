using MongoDB.Driver;
using Serilog;
using System;

namespace PatientHistory.Data
{
    public class DbInitializer
    {
        public static void Initialize(NoteContext context)
        {
            Log.Debug("DbInitializer : Initialize()");

            var list = context.Notes.Find(x => true).ToList();
            if (list.Count is not 0)
            {
                return;
            }

            var notes = new Note[]
            {
                new()
                {
                    PatientId = 1,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they are \"feeling terrific\"\nWeight at or below recommended level",
                },
                new()
                {
                    PatientId = 1,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they feel tired during the day\nPatient also complains about muscle aches\nLab reports Microalbumin elevated",
                },
                new()
                {
                    PatientId = 1,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they not feeling as tired\nSmoker, quit within last year\nLab results indicate Antibodies present elevated",
                },
                new()
                {
                    PatientId = 2,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they are feeling a great deal of stress at work\nPatient also complains that their hearing seems Abnormal as of late"
                    ,
                },
                new()
                {
                    PatientId = 2,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they have had a Reaction to medication within last 3 months\nPatient also complains that their hearing continues to be Abnormal"
                },
                new()
                {
                    PatientId = 2,
                    CreatedTime = DateTime.Now,
                    Message = "Lab reports Microalbumin elevated",
                },
                new()
                {
                    PatientId = 2,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that everything seems fine\nLab reports Hemoglobin A1C above recommended level\nPatient admits to being long term Smoker",
                },
                new()
                {
                    PatientId = 3,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they are short term Smoker",
                },
                new()
                {
                    PatientId = 3,
                    CreatedTime = DateTime.Now,
                    Message = "Lab reports Microalbumin elevated",
                },
                new()
                {
                    PatientId = 3,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they are a Smoker, quit within last year\nPatient also complains that of Abnormal breathing spells\nLab reports Cholesterol LDL high",
                },
                new()
                {
                    PatientId = 3,
                    CreatedTime = DateTime.Now,
                    Message = "Lab reports Cholesterol LDL high",
                },
                new()
                {
                    PatientId = 4,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that walking up stairs has become difficult\nPatient also complains that they are having shortness of breath\nLab results indicate Antibodies present elevated\nReaction to medication"
                },
                new()
                {
                    PatientId = 4,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they are experiencing back pain when seated for a long time",
                },
                new()
                {
                    PatientId = 4,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they are a short term Smoker\nHemoglobin A1C above recommended level"
                },
                new()
                {
                    PatientId = 5,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they experiencing occasional neck pain\nPatient also complains that certain foods now taste different\nApparent Reaction to medication\nBody Weight above recommended level"
                },
                new()
                {
                    PatientId = 5,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they had multiple dizziness episodes since last visit\nBody Height within concerned level"
                },
                new()
                {
                    PatientId = 5,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they are still experiencing occaisional neck pain\nLab reports Microalbumin elevated\nSmoker, quit within last year"
                },
                new()
                {
                    PatientId = 5,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they had multiple dizziness episodes since last visit\nLab results indicate Antibodies present elevated"
                },
                new()
                {
                    PatientId = 6,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they feel fine\nBody Weight above recommended level"
                },
                new()
                {
                    PatientId = 6,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they feel fine"
                },
                new()
                {
                    PatientId = 7,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they often wake with stiffness in joints\nPatient also complains that they are having difficulty sleeping\nBody Weight above recommended level\nLab reports Cholesterol LDL high"
                },
                new()
                {
                    PatientId = 8,
                    CreatedTime = DateTime.Now,
                    Message = "Lab results indicate Antibodies present elevated\nHemoglobin A1C above recommended level"
                },
                new()
                {
                    PatientId = 9,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they are having trouble concentrating on school assignments\nHemoglobin A1C above recommended level"
                },
                new()
                {
                    PatientId = 9,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they frustrated as long wait times\nPatient also complains that food in the vending machine is sub-par\nLab reports Abnormal blood cell levels"
                },
                new()
                {
                    PatientId = 9,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they are easily irritated at minor things\nPatient also complains that neighbors vacuuming is too loud\nLab results indicate Antibodies present elevated"
                },
                new()
                {
                    PatientId = 10,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they are not experiencing any problems"
                },
                new()
                {
                    PatientId = 10,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they are not experiencing any problems\nBody Height within concerned level\nHemoglobin A1C above recommended level"
                },
                new()
                {
                    PatientId = 10,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they are not experiencing any problems\nBody Weight above recommended level\nPatient reports multiple dizziness episodes since last visit"
                },
                new()
                {
                    PatientId = 10,
                    CreatedTime = DateTime.Now,
                    Message = "Patient states that they are not experiencing any problems\nLab reports Microalbumin elevated"
                },
            };
            Log.Debug("DbInitializer : Seeding Db ...");
            context.Notes.InsertMany(notes);
        }
    }
}
