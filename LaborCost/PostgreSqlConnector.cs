using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace LaborCost
{
    class PostgreSqlConnector
    {
        private string connectionString = "Server=localhost;Port=1999;" +
                "Database=TBP;User Id=postgres;Password=Fulton1807;";



        public List<Employee> GetAll_Employees()
        {
            List<Employee> employees = new List<Employee>();

            NpgsqlConnection connection =
                 new NpgsqlConnection(connectionString);

            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;


            command.CommandType = System.Data.CommandType.Text;

            command.CommandText = "select * from employee";

            NpgsqlDataReader dataReader = command.ExecuteReader();


            while (dataReader.Read())
            {
                Employee employee = new Employee();
                employee.Id = (int)dataReader["id"];
                employee.FirstName = (string)dataReader["first_name"];
                employee.LastName = (string)dataReader["last_name"];
                employee.Email = (string)dataReader["email"];
                employee.PhoneNumber = (string)dataReader["phone_number"];
                employee.BirthDay = (DateTime)dataReader["birth_date"];
                employee.Adress = (string)dataReader["adress"];
                employee.Username = (string)dataReader["username"];
                employee.Password = (string)dataReader["password"];
                employee.Role = (int)dataReader["role"];
                employees.Add(employee);
            }

            connection.Close();
            return employees;

        }

        public void Insert_Employee(Employee employee)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO employee (first_name,last_name,email," +
                        "phone_number,birth_date,adress,username,password,role) VALUES (@firstName, @lastName," +
                        "@email, @phoneNumber, @birthDate, @adress, @username, @password, @role)";

                    cmd.Parameters.AddWithValue("@firstName", employee.FirstName);

                    cmd.Parameters.AddWithValue("@lastName", employee.LastName);

                    cmd.Parameters.AddWithValue("@email", employee.Email);

                    cmd.Parameters.AddWithValue("@phoneNumber", employee.PhoneNumber);

                    cmd.Parameters.AddWithValue("@birthDate", employee.BirthDay);

                    cmd.Parameters.AddWithValue("@adress", employee.Adress);

                    cmd.Parameters.AddWithValue("@username", employee.Username);

                    cmd.Parameters.AddWithValue("@password", employee.Password);

                    cmd.Parameters.AddWithValue("@role", employee.Role);

                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public Employee SearchEmployee(int id_Employee)
        {
            Employee employee = new Employee();



            List<Employee> employees = GetAll_Employees();

            foreach (var employee1 in employees)
            {
                if(employee1.Id == id_Employee)
                {
                    employee = employee1;
                }
            }
            return employee;
        }
        
        public List<JobPosition> getAllJobPosition()
        {
            List<JobPosition> jobPositions = new List<JobPosition>();

            NpgsqlConnection connection =
               new NpgsqlConnection(connectionString);

            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;


            command.CommandType = System.Data.CommandType.Text;

            command.CommandText = "select * from job_position";

            NpgsqlDataReader dataReader = command.ExecuteReader();


            while (dataReader.Read())
            {
                JobPosition job = new JobPosition();
                job.Id = (int)dataReader["id_job"];
                job.Name = (string)dataReader["name"];
                jobPositions.Add(job);

            }
            connection.Close();
            return jobPositions;
        }

        public void Insert_Contract(Contract contract)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO contract (employee,job_position,start_contract," +
                        "price_of_hour,number_of_vacation_days) VALUES (@employee, @job_position," +
                        "@start_contract,@price_of_hour, @number_of_vacation_days)";

                    cmd.Parameters.AddWithValue("@employee", contract.Employee.Id);

                    cmd.Parameters.AddWithValue("@job_position", contract.Job.Id);

                    cmd.Parameters.AddWithValue("@start_contract", contract.Start_Contract);

                    cmd.Parameters.AddWithValue("@price_of_hour", contract.Price_of_hour);

                    cmd.Parameters.AddWithValue("@number_of_vacation_days", contract.Number_of_Vacation_Days);

                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public JobPosition SearchJob(int jobId)
        {
            JobPosition Searchedjob = new JobPosition();

            foreach (var job in getAllJobPosition())
            {
                if(job.Id == jobId)
                {
                    Searchedjob = job;
                }
            }


            return Searchedjob;
        }

        public List<Contract> Get_All_Contracts()
        {
            List<Contract> contracts = new List<Contract>();

            

            NpgsqlConnection connection =
                 new NpgsqlConnection(connectionString);

            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;


            command.CommandType = System.Data.CommandType.Text;

            command.CommandText = "select * from contract";

            NpgsqlDataReader dataReader = command.ExecuteReader();


            while (dataReader.Read())
            {
                Contract contract = new Contract();
                contract.Id = (int)dataReader["id"];
                contract.Employee = SearchEmployee((int)dataReader["employee"]);
                contract.Job = SearchJob((int)dataReader["job_position"]);
                contract.Start_Contract = (DateTime)dataReader["start_contract"];
                contract.Price_of_hour = (decimal)dataReader["price_of_hour"];
                contract.Number_of_Vacation_Days = (int)dataReader["number_of_vacation_days"];
                contracts.Add(contract);           
            }

            connection.Close();

            return contracts;
        }
        public Contract Get_Contract(int emp_ID)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;


            command.CommandType = System.Data.CommandType.Text;

            command.CommandText = "select * from contract where FK1_ID_Employee = "+emp_ID+" order by Start_Contract desc";
            //we only need the latest contract to fid the one that curretly applies to te employee (right?)
            NpgsqlDataReader dataReader = command.ExecuteReader();


                Contract contract = new Contract();
                contract.Id = (int)dataReader["id"];
                contract.Employee = SearchEmployee((int)dataReader["employee"]);
                contract.Job = SearchJob((int)dataReader["job_position"]);
                contract.Start_Contract = (DateTime)dataReader["start_contract"];
                contract.Price_of_hour = (decimal)dataReader["price_of_hour"];
                contract.Number_of_Vacation_Days = (int)dataReader["number_of_vacation_days"];
                contracts.Add(contract);

            connection.Close();

            return contract;
        }

        public void Update_Contract(decimal price_of_hour, Employee employee)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE contract SET price_of_hour = @price_of_hour WHERE employee = @id";
                    command.Parameters.AddWithValue("@price_of_hour", price_of_hour);
                    command.Parameters.AddWithValue("@id", employee.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Insert_Job(JobPosition job)
        {

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO job_position (name) VALUES (@name)";

                    cmd.Parameters.AddWithValue("@name", job.Name);

                    cmd.ExecuteNonQuery();
                }

                conn.Close();


            }
        }


        public List<Shift> Get_All_Shifts()
        {
            List<Shift> shifts = new List<Shift>();


            NpgsqlConnection connection =
              new NpgsqlConnection(connectionString);

            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;


            command.CommandType = System.Data.CommandType.Text;

            command.CommandText = "select * from shift";

            NpgsqlDataReader dataReader = command.ExecuteReader();


            while (dataReader.Read())
            {
                Shift shift = new Shift();

                shift.Id = (int)dataReader["id"];
               
                shift.Employee = SearchEmployee((int)dataReader["employee"]);

                shift.Date_of_shift = (DateTime)dataReader["date_of_shift"];

              shift.Type_of_shift = SearchShiftType((int)dataReader["shift_type"]);
                
                shifts.Add(shift);

            }
            connection.Close();
           return shifts;
        }


        public List<Shift_type> Get_All_Shift_Types()
        {
            List<Shift_type> Type_of_shifts = new List<Shift_type>();


            NpgsqlConnection connection =
              new NpgsqlConnection(connectionString);

            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;


            command.CommandType = System.Data.CommandType.Text;

            command.CommandText = "select * from shift_type";

            NpgsqlDataReader dataReader = command.ExecuteReader();


            while (dataReader.Read())
            {
                Shift_type shift = new Shift_type();

                shift.Id = (int)dataReader["id"];

                shift.Type = (string)dataReader["type"];

                shift.Payweight = (decimal)dataReader["payweight"];

                shift.Start_time = (TimeSpan)dataReader["start_time"];

                shift.End_time = (TimeSpan)dataReader["end_time"];

                Type_of_shifts.Add(shift);

            }
            connection.Close();
            return Type_of_shifts;
        }

        public int CountShiftType(int emp_ID, string type)
        {
            int num;


            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;


            command.CommandType = System.Data.CommandType.Text;

            command.CommandText = "select Count(ID_Shift_Type) from shift_type join shift on ID_Shift=ID_Typr_Shift where FK1_ID_Employee = "+emp_ID" and type = "+type";

            NpgsqlDataReader dataReader = command.ExecuteReader();


            dataReader.Read()
           
            num = (int)dataReader["Count(ID_Shift_Type)"];

            connection.Close();

            return num;
        }
        public Shift_type SearchShiftType(int idShift_Type)
        {
            Shift_type shift = new Shift_type();


            foreach (var shift_type in Get_All_Shift_Types())
            {
                if(shift_type.Id == idShift_Type)
                {
                    shift = shift_type;
                }
            }

            return shift;

        }


        public List<Mode_of_operation> GetAllModes_of_Operation()
        {
            List<Mode_of_operation> modes = new List<Mode_of_operation>();

            NpgsqlConnection connection =
              new NpgsqlConnection(connectionString);

            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;


            command.CommandType = System.Data.CommandType.Text;

            command.CommandText = "select * from mode_of_operation";

            NpgsqlDataReader dataReader = command.ExecuteReader();


            while (dataReader.Read())
            {
                Mode_of_operation mod = new Mode_of_operation();

                 mod.Id = (int)dataReader["id"];

                mod.Type = (string)dataReader["type"];

                mod.Payweight = (decimal)dataReader["payweight"];

                modes.Add(mod);

            }
            connection.Close();
            return modes;



        }

        public Mode_of_operation SearchMode(int modId)
        {
            Mode_of_operation SearchedMode = new Mode_of_operation();

            foreach (var mod in GetAllModes_of_Operation())
            {
                if(mod.Id == modId)
                {
                    SearchedMode = mod;
                }
            }
            return SearchedMode;
        }


        public List<Leave> Get_All_Leaves()
        {

            List<Leave> Leaves = new List<Leave>();

            NpgsqlConnection connection =
              new NpgsqlConnection(connectionString);

            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;


            command.CommandType = System.Data.CommandType.Text;

            command.CommandText = "select * from leave_managment";

            NpgsqlDataReader dataReader = command.ExecuteReader();


            while (dataReader.Read())
            {
                Leave leave = new Leave();

                leave.Id = (int)dataReader["id"];

                leave.Employee = SearchEmployee((int)dataReader["employee"]);

                leave.Mode = SearchMode((int)dataReader["mode_of_operation"]);

                leave.Start_date = (DateTime)dataReader["start_date"];

                leave.End_date = (DateTime)dataReader["end_date"];

                leave.Reason = (string)dataReader["reason"];

                Leaves.Add(leave);       
            }
            connection.Close();
            return Leaves;
        }


        public List<Paycheck> GetAllPaychecks()
        {
            List<Paycheck> paychecks = new List<Paycheck>();

            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand();

            command.Connection = connection;

            command.CommandType = System.Data.CommandType.Text;

            command.CommandText = "select * from paycheck";

            NpgsqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                Paycheck paycheck = new Paycheck();

                paycheck.Id = (int)dataReader["id"];

                paycheck.Employee = SearchEmployee((int)dataReader["employee"]);

                paycheck.PaymentDate = (DateTime)dataReader["payment_date"];

                paycheck.Total_sick_leave_days = (int)dataReader["total_sick_leave_days"];

                paycheck.Total_vacation_days = (int)dataReader["total_vacation_days"];

                paycheck.Total_hours = (int)dataReader["total_hours"];

                paycheck.Total_overtime_hours = (int)dataReader["total_overtime_hours"];

                paycheck.Gross_salary = (decimal)dataReader["gross_salary"];

                paycheck.Contributions = (decimal)dataReader["contributions"];

                paycheck.Net_Salary = (decimal)dataReader["net_salary"];

                paycheck.Date_From = (DateTime)dataReader["date_from"];

                paycheck.Date_To = (DateTime)dataReader["date_to"];

            }
            connection.Close();
            return paychecks;
        }

        public Paycheck GetLastPaycheck(Employee employee)
        {

            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand();

            command.Connection = connection;

            command.CommandType = System.Data.CommandType.Text;

            command.CommandText = "select * from paycheck where FK1_ID_Employee = "+employee.ID+" oder by paymentdate Desc";
            //ordering all the paycheck of one employee by their payment date to know when was the last time waid employee got paid
            NpgsqlDataReader dataReader = command.ExecuteReader();

            dataReader.Read()
            
            Paycheck paycheck = new Paycheck();

            paycheck.Id = (int)dataReader["id"];

            paycheck.Employee = SearchEmployee((int)dataReader["employee"]);

            paycheck.PaymentDate = (DateTime)dataReader["payment_date"];

            paycheck.Total_sick_leave_days = (int)dataReader["total_sick_leave_days"];

            paycheck.Total_vacation_days = (int)dataReader["total_vacation_days"];

            paycheck.Total_hours = (int)dataReader["total_hours"];

            paycheck.Total_overtime_hours = (int)dataReader["total_overtime_hours"];

            paycheck.Gross_salary = (decimal)dataReader["gross_salary"];

            paycheck.Contributions = (decimal)dataReader["contributions"];

            paycheck.Net_Salary = (decimal)dataReader["net_salary"];

            paycheck.Date_From = (DateTime)dataReader["date_from"];

            paycheck.Date_To = (DateTime)dataReader["date_to"];

            
            connection.Close();
            return paycheck;
        }
    }
}
