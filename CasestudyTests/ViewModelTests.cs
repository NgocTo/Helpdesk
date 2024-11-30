using HelpdeskViewModels;
using Xunit.Abstractions;

namespace CasestudyTests
{
    public class ViewModelTests
    {

        private readonly ITestOutputHelper output;
        public ViewModelTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task Employee_ComprehensiveVMTest()
        {
            EmployeeViewModel evm = new()
            {
                Title = "Mr.",
                Firstname = "Some",
                Lastname = "Employee",
                Email = "some@abc.com",
                Phoneno = "(777)777-7777",
                DepartmentId = 100 // ensure department id is in Departments table
            };
            await evm.Add();
            output.WriteLine("New Employee Added - Id = " + evm.Id);
            int? id = evm.Id; // need id for delete later
            await evm.GetById((int)id!);
            output.WriteLine("New Employee " + id + " Retrieved");
            evm.Phoneno = "(555)555-1233";
            if (await evm.Update() == 1)
            {
                output.WriteLine("Employee " + id + " phone# was updated to - " + evm.Phoneno);
            }
            else
            {
                output.WriteLine("Employee " + id + " phone# was not updated!");
            }
            evm.Phoneno = "Another change that should not work";
            if (await evm.Update() == -2)
            {
                output.WriteLine("Employee " + id + " was not updated due to stale data");
            }
            evm = new EmployeeViewModel
            {
                Id = id
            };
            // need to reset because of concurrency error
            await evm.GetById((int)id!);
            if (await evm.Delete() == 1)
            {
                output.WriteLine("Employee " + id + " was deleted!");
            }
            else
            {
                output.WriteLine("Employee " + id + " was not deleted");
            }
            // should throw expected exception
            Task<NullReferenceException> ex = Assert.ThrowsAsync<NullReferenceException>(async () => await evm.GetById((int)id!));
        }



        [Fact]
        public async Task Call_ComprehensiveVMTest()
        {
            EmployeeViewModel evm = new();
            await evm.GetByLastName("To");
            EmployeeViewModel tvm = new();
            await tvm.GetByLastName("Burner");
            ProblemViewModel pvm = new();
            await pvm.GetByDescription("Memory Upgrade");

            CallViewModel cvm = new()
            {
                EmployeeId = (int)evm.Id!,
                TechId = (int)tvm.Id!,
                ProblemId = (int)pvm.Id!,
                DateOpened = DateTime.Now,
                OpenStatus = true,
                Notes = evm.Firstname + evm.Lastname + " has bad RAM, " + tvm.Lastname + " to fix it"
            };

            await cvm.Add();
            output.WriteLine("New Call Added - Id = " + cvm.Id);
            int? id = cvm.Id; // need id for delete later
            cvm = new CallViewModel();
            await cvm.GetById((int)id!);
            output.WriteLine("New Call " + id + " Retrieved");
            cvm.Notes += "\n Ordered new RAM!";
            if (await cvm.Update() == 1)
            {
                output.WriteLine("Call " + id + " notes was updated to - " + cvm.Notes);
            }
            else
            {
                output.WriteLine("Call " + id + " notes was not updated!");
            }
            cvm.Notes = "Another change that should not work";
            if (await cvm.Update() == -2)
            {
                output.WriteLine("Call " + id + " was not updated due to stale data");
            }
            cvm = new CallViewModel
            {
                Id = (int)id
            };
            // need to reset because of concurrency error
            await cvm.GetById((int)id!);
            if (await cvm.Delete((int)id) == 1)
            {
                output.WriteLine("Call " + id + " was deleted!");
            }
            else
            {
                output.WriteLine("Call " + id + " was not deleted");
            }
            // should throw expected exception
            Task<NullReferenceException> ex = Assert.ThrowsAsync<NullReferenceException>(async () => await cvm.GetById((int)id!));
        }



        [Fact]
        public async Task Employee_GetByEmailTest()
        {
            EmployeeViewModel vm = new();
            await vm.GetByEmail("n_to239383@fanshaweonline.ca");
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public async Task Employee_GetByIdTest()
        {
            EmployeeViewModel vm = new();
            await vm.GetById(1004);
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public async Task Employee_GetAllTest()
        {
            List<EmployeeViewModel> allEmployeeVms;
            EmployeeViewModel vm = new();
            allEmployeeVms = await vm.GetAll();
            Assert.True(allEmployeeVms.Count > 0);
        }
        [Fact]
        public async Task Employee_AddTest()
        {
            EmployeeViewModel vm;
            vm = new()
            {
                Title = "Mrs.",
                Firstname = "Ngoc",
                Lastname = "To",
                Email = "n_to239383@fanshaweonline.ca",
                Phoneno = "(777)777-7777",
                DepartmentId = 100
            };
            await vm.Add();
            Assert.True(vm.Id > 0);
        }
        [Fact]
        public async Task Employee_UpdateTest()
        {
            EmployeeViewModel vm = new();
            await vm.GetByEmail("n_to239383@fanshaweonline.ca"); // Employee just added in Add test
            vm.Email = vm.Email == "n_to239383@fanshaweonline.ca" ? "default@fanshaweonline.ca" : "n_to239383@fanshaweonline.ca";
            // will be -1 if failed 0 if no data changed, 1 if succcessful
            Assert.True(await vm.Update() == 1);
        }
        [Fact]
        public async Task Employee_DeleteTest()
        {
            EmployeeViewModel vm = new();
            await vm.GetByEmail("n_to239383@fanshaweonline.ca"); // Employee just added
            Assert.True(await vm.Delete() == 1); // 1 employee deleted
        }
    }
}
