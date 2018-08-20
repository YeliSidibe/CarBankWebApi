delete from dbo.AspNetUsers; 
delete from dbo.Customers;

select a.* from dbo.AspNetUsers a join Customers c
on a.Id = c.IdentityId;

update dbo.AspNetUsers set AccessFailedCount = 0,LockoutEnabled = 0;