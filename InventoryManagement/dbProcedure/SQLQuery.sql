alter table tbl_magzine_release add dtitle nvarchar(100)
go
update tbl_magzine_release set dtitle='' where dtitle is null
go
-------------------------------------------------------------------------
alter table tbl_magzine_release add stock int
go
update tbl_magzine_release set stock=0 where stock is null
go
-------------------------------------------------------------------------
alter table tbl_magzine add gstper decimal(5,2)
go
update tbl_magzine set gstper=0 where gstper is null
go
-------------------------------------------------------------------------
alter table tbl_magzine add frequency char(1)
go
update tbl_magzine set frequency='D' where frequency is null
go
------------------------------------------------------------------------
alter table tbl_users add loginCnt int
go
update tbl_users set loginCnt=0 where loginCnt is null
go
------------------------------------------------------------------------
alter table tbl_users add IsLocked char(1)
go
update tbl_users set IsLocked='N' where IsLocked is null
go
------------------------------------------------------------------------
alter table tbl_users add ustatus bit
go
update tbl_users set ustatus=1 where ustatus is null
go
------------------------------------------------------------------------
alter table tbl_users add registered datetime, Latest_Activity datetime
go
update tbl_users set registered='2001-01-01',Latest_Activity='2001-01-01' where Latest_Activity is null
go
---------------------------------------------------------------------------------------------------------
select * from tbl_membership

alter table tbl_membership add partytype char(1)
go
update tbl_membership set partytype='m' 
go

alter table tbl_membership add gstin char(30)
go
update tbl_membership set gstin='0' 
go

alter table tbl_membership add panno char(20)
go
update tbl_membership set panno='0' 
go

alter table tbl_membership add contactperson char(100)
go
update tbl_membership set contactperson='0' 
go
-------------------------------------------------------------

alter table tbl_master add trantype char(2)
go
update tbl_master set trantype='0' 
go
------------------------------------------------------------
alter table tbl_master_ledger add trantype char(2)
go
update tbl_master_ledger set trantype='0' 
go
-----------------------------------------------------------
alter table tbl_master_ledger add lstatsu char(1)
go
update tbl_master_ledger set lstatsu='0' 
go
-----------------------------------------------------------

alter table tbl_master add partyid  int
go
update tbl_master set partyid='0' 
go



