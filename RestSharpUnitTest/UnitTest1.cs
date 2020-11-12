using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace RestSharpUnitTest
{
    /// <summary>
    /// Creating the class to take the different entries of an employee
    /// </summary>
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string salary { get; set; }
    }
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Calling RestClient class
        /// </summary>
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }
        /// <summary>
        /// UC 1:
        /// Setting up the method to get all the employees.
        /// </summary>
        /// <returns></returns>
        private IRestResponse GetEmployeeList()
        {
            //Arrange
            RestRequest request = new RestRequest("/employees", Method.GET);

            //Act
            IRestResponse response = client.Execute(request);
            return response;
        }
        [TestMethod]
        public void OnCallingGETApi_ShouldReturnEmployeeList()
        {
            IRestResponse response = GetEmployeeList();

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(3, dataResponse.Count);
            foreach(Employee employee in dataResponse)
            {
                System.Console.WriteLine("ID: " + employee.id + " Name: " + employee.name + " Salary: " + employee.salary);
            }
        }
    }
}
