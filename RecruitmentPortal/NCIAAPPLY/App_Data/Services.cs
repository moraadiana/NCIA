using NCIAAPPLY.NAVWS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using NCIAAPPLY.ViewModels;
using System.Reflection;
using System.Web.UI.WebControls.WebParts;

namespace NCIAAPPLY
{
    public class Services
    {
        public static SqlConnection connection;
        public static SqlCommand command;
        public static SqlDataReader reader;


        public static List<AdvertisedJobs> GetAdvertisedJobs()
        {
            var advertisedJobs = new List<AdvertisedJobs>();
            try
            {
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetAdvertisedJobs",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.CompanyName);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    int counter = 0;
                    while (reader.Read())
                    {
                        counter++;
                        var job = new AdvertisedJobs()
                        {
                            Counter = counter,
                            JobRefNo = reader["Requisition No_"].ToString(),
                            JobTitle = reader["Job Description"].ToString(),
                            JobId = reader["Job ID"].ToString(),
                            VacantPositions = Convert.ToDecimal(reader["Vacant Positions"].ToString()),
                            RequiredPositions = Convert.ToDecimal(reader["Required Positions"].ToString()),

                        };
                        advertisedJobs.Add(job);
                    }
                }
               
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return advertisedJobs;
        }

        public static List<JobRequirement> GetJobRequirements(string jobId)
        {
            List<JobRequirement> jobRequirements = new List<JobRequirement>();
            try
            {
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetJobRequirements",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.CompanyName);
                command.Parameters.AddWithValue("JobID", "'"+jobId+"'");
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    int counter = 0;
                    while (reader.Read())
                    {
                        counter++;
                        var req = new JobRequirement()
                        {
                            Requirements = reader["Qualification Description"].ToString()
                        };
                        jobRequirements.Add(req);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return jobRequirements;
        }

        public static List<JobResponsibility> GetJobResponsibilities(string jobId)
        {
            List<JobResponsibility> jobResponsibilities = new List<JobResponsibility>();
            try
            {
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetJobResponsibilities",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.CompanyName);
                command.Parameters.AddWithValue("JobID", "'"+jobId+"'");
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    int counter = 0;
                    while (reader.Read())
                    {
                        counter++;
                        var req = new JobResponsibility()
                        {
                            Responsibilities = reader["Responsibility Description"].ToString()
                        };
                        jobResponsibilities.Add(req);
                    }
                }
                
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return jobResponsibilities;
        }

        public static string GetJobDescription(string jobId)
        {
            string jobDescription = string.Empty;
            try
            {
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetJobDescription",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.CompanyName);
                command.Parameters.AddWithValue("JobID", jobId);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    
                    while (reader.Read())
                    {
                        jobDescription = reader["Job Description"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return jobDescription;
        }

        public static List<Citizenship> GetCitizenships()
        {
            List<Citizenship> citizenships = new List<Citizenship>();
            try
            {
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetCitizenship",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.CompanyName);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        var citizen = new Citizenship()
                        {
                            Code = reader["Code"].ToString(),
                            Name = reader["Name"].ToString()
                        };
                        citizenships.Add(citizen);
                    }
                }
                
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return citizenships;
        }

        public static List<MyApplication> GetMyApplications(string username)
        {
            var myApplications = new List<MyApplication>();
            try
            {
                var delimiter1 = new string [] { "::" };
                var delimiter2 = new string [] { "[]" };
                int counter = 0;
                var response = Components.ObjNav.GetMyApplications(username);
                if (response != null)
                {
                    var firstPart = response.Split(delimiter2, StringSplitOptions.None);
                    foreach (var part in firstPart)
                    {
                       var secondPart =  part.Split(delimiter1, StringSplitOptions.None);
                        
                            counter++;

                            var application = new MyApplication()
                            {
                                Counter = counter,
                                JobApplicationNo = secondPart[0],
                                JobId = secondPart[1],
                                JobRefNo = secondPart[2],
                                JobTitle = secondPart[3] ,
                                DateApplied = secondPart[4] ,
                                Status = secondPart[5] ,
                            };
                        myApplications.Add(application);
                        
                    }
                }

            }
            catch (Exception ex) { ex.Data.Clear(); }
            return myApplications;
        }

        public static List<ApplicantQualification> GetApplicantQualifications(string applicationNo)
        {
            var applicantQualifications = new List<ApplicantQualification>();
            try
            {
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetQualifications",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.CompanyName);
                command.Parameters.AddWithValue("@Job Application No", applicationNo);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    int counter = 0;
                    while (reader.Read())
                    {
                        counter++;
                        ApplicantQualification qualification = new ApplicantQualification()
                        {
                            Counter = counter,
                            ApplitionNo = reader["Application No"].ToString(),
                            QualificationType = reader["Qualification Type"].ToString(),
                            QualificationCode = reader["Qualification Code"].ToString(),
                            QualificationDescription = reader["Qualification Description"].ToString(),
                            Institution = reader["Institution_Company"].ToString(),
                            StartDate = reader["From Date"].ToString(),
                            EndDate = reader["To Date"].ToString()
                        };
                        applicantQualifications.Add(qualification);

                    }
                }
                
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return applicantQualifications;
        }        

        public static List<ApplicantReferee> GetApplicantReferee(string applicationNo)
        {
            var applicantReferees = new List<ApplicantReferee>();
            try
            {
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetReferees",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.CompanyName);
                command.Parameters.AddWithValue("@JobID", "'" + applicationNo+"'");
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    int counter = 0;
                    while (reader.Read())
                    {
                        counter++;
                        ApplicantReferee referee = new ApplicantReferee()
                        {
                            Counter = counter,
                            FullName = reader["Names"].ToString(),
                            Institution = reader["Institution"].ToString(),
                            Designation = reader["Designation"].ToString(),
                            Email = reader["E-Mail"].ToString(),
                            Phone = reader["Telephone No"].ToString()
                        };
                        applicantReferees.Add(referee);
                       
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return applicantReferees;
        }

        public static List<ApplicantHobby> GetApplicantHobbies(string applicationNo)
        {
            var applicantHobbies = new List<ApplicantHobby>();
            try
            {
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetHobbies",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.CompanyName);
                command.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    int counter = 0;
                    while (reader.Read())
                    {
                        counter++;
                        ApplicantHobby hobby = new ApplicantHobby()
                        {
                            Counter = counter,
                            ApplicantNo = reader["Job Application No"].ToString(),
                            Hobbies = reader["Hobby"].ToString()
                        };
                        applicantHobbies.Add(hobby);

                    }
                }
                
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return applicantHobbies;
        }

        public static List<ApplicantAttachments> GetApplicantAttachments(string applicationNo)
        {
            var applicantAttachments = new List<ApplicantAttachments>();
            try
            {
                connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetDocumentAttachments",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.CompanyName);
                command.Parameters.AddWithValue("@DocNo", "'" + applicationNo + "'");
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    int counter = 0;
                    while(reader.Read())
                    {
                        counter++;
                        ApplicantAttachments attachments = new ApplicantAttachments()
                        {
                            Counter = counter,
                            Description = reader["File Name"].ToString() +"."+ reader["File Extension"].ToString(),
                            DocumentNo = reader["No_"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["Attached Date"].ToString()),
                            SystemId = reader["$systemId"].ToString()
                        };
                        applicantAttachments.Add(attachments);
                    }
                }
                connection.Close();
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return applicantAttachments;
        }

        public static List<QualificationType> GetQualificationTypes(string jobId)
        {
            var qualificationTypes = new List<QualificationType>();
            try
            {
                /*connection = Components.GetconnToNAV();
                command = new SqlCommand()
                {
                    CommandText = "spGetQualificationTypes",
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection
                };
                command.Parameters.AddWithValue("@Company_Name", Components.CompanyName);
                command.Parameters.AddWithValue("@Job Id", "'" + jobId + "'");
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    
                    while (reader.Read())
                    {
                        QualificationType type = new QualificationType()
                        {
                            Code = reader["Qualification Category"].ToString()
                        };
                        qualificationTypes.Add(type);

                    }
                }*/
                string response = Components.ObjNav.GetJobQualificationType(jobId);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(new string[] { "[]" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in responseArr)
                    {
                        string[] qualificationArr = str.Split(new string[] { "::" }, StringSplitOptions.None);
                        QualificationType type = new QualificationType()
                        {
                            Code = qualificationArr[0]
                        };
                        qualificationTypes.Add(type);
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return qualificationTypes;
        }

        public static List<QualificationCode> GetQualificationCodes(string jobId, string code)
        {
            var qualificationCode = new List<QualificationCode>();
            try
            {

                string qualificationCodes = Components.ObjNav.GetJobQualificationCodes(jobId, code);
                if (!string.IsNullOrEmpty(qualificationCodes))
                {
                    string[] qualificationCodesArr = qualificationCodes.Split(new string[] { "[]" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in qualificationCodesArr)
                    {
                        string[] responseArr = str.Split(new string[] { "::" }, StringSplitOptions.None);
                        QualificationCode code1 = new QualificationCode()
                        {
                            Code = responseArr[0],
                            Description = responseArr[1]
                        };
                        qualificationCode.Add(code1);
                    }
                }
            }
            catch(Exception ex)
            {
                ex.Data.Clear();
            }
            return qualificationCode;
        }
    }
}