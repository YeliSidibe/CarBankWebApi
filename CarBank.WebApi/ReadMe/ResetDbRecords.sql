delete from dbo.AspNetUsers; 
delete from dbo.Customers;

select * from dbo.AspNetUsers a join Customers c
on a.Id = c.IdentityId;