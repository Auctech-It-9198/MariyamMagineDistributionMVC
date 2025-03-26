use Maryamsoft
go
-------------------------------------------------------
alter  procedure usp_generate_new_id
@tblname varchar(50),
@colname varchar(max)
as
declare @sql nvarchar(max)
set @sql='select isnull(max(' + @colname + ')+1,1) as nid from ' + @tblname
exec sp_executesql @sql
go
--------------------------------------------------------

alter  procedure usp_generate_new_id_vtype
@tblname varchar(50),
@colname varchar(max),
@vtype char(2)
as
declare @sql nvarchar(max)
set @sql='select isnull(max(' + @colname + ')+1,1) as nid from ' + @tblname + ' where vtype='''+@vtype+''''
exec sp_executesql @sql
go


--------------------------------------------------------
---spMenue
--------------------------------------------------------
alter procedure usp_get_tbl_menu_by_userid
@compcode int,
@userid int
as
select c.*,isnull(u.menuper,0)as menuper,isnull(u.approve,0)as approve from tbl_menu as c 
left join tbl_menu_permission as u  on
(u.mid=c.mid and c.compcode=u.compcode)
where u.userid=@userid and u.compcode=@compcode
order by c.morder
go
--------------------------------------------------------
alter procedure usp_display_tbl_menu
@compcode int,
@userid int
as
select c.*,isnull(u.menuper,0)as menuper,isnull(u.approve,0)as approve from tbl_menu as c 
left join tbl_menu_permission as u  on
(u.mid=c.mid and c.compcode=u.compcode)
where  c.compcode=@compcode
order by c.morder
go
--------------------------------------------------------
alter procedure usp_get_tbl_menu
@compcode int,
@userid int,
@mid int
as
select c.*,
menuper=isnull((select menuper from tbl_menu_permission where mid=@mid and compcode=@compcode and userid=@userid),0),
approve=isnull((select approve from tbl_menu_permission where mid=@mid and compcode=@compcode and userid=@userid),0)
from tbl_menu as c 
--left join tbl_menu_permission as u  on(u.mid=c.mid and c.compcode=u.compcode and u.userid=@userid)
where c.compcode=@compcode and c.mid=@mid
order by c.morder
go
--------------------------------------------------------
alter procedure usp_display_tbl_smenu
@compcode int,
@userid int,
@mid int
as
select up.mid,m.*,
addper=isnull(addper,0),
editper=isnull(editper,0),
deleteper=isnull(deleteper,0),
viewper=isnull(viewper,0),
printper=isnull(printper,0),
menuper=isnull(menuper,0)
from tbl_smenu as m 
left join tbl_userpermission as up on(m.compcode=up.compcode and m.mid=up.mid and m.smid=up.smid)
where m.mid=@mid order by smorder
go
--------------------------------------------------------
----spCompUsers
--------------------------------------------------------
alter procedure usp_insert_tbl_compusers
@userid int,
@compcode tinyint
as
insert into tbl_compusers
(
userid,compcode
)
values(
@userid,
@compcode
)
go
--------------------------------------------------------
alter procedure usp_get_tbl_compusers
@userid int
as
select cu.*,c.cmpname from tbl_compusers as cu
left join tbl_company as c on(cu.compcode=c.compcode)
where userid=@userid
go
--------------------------------------------------------
alter procedure usp_delete_tbl_compusers
@userid int
as
delete from tbl_compusers where userid=@userid
go
--------------------------------------------------------
---spUsers
--------------------------------------------------------
alter procedure usp_login_tbl_users
@username varchar(100),
@passw varchar(50)
AS
BEGIN
BEGIN	
-- USER NOT EXISTS
IF NOT EXISTS(SELECT 1 FROM tbl_users WHERE username=@username)
BEGIN
			SELECT 'User not exists .' AS RES RETURN;
END
-- USER INACTIVE
IF EXISTS (	SELECT 1 FROM tbl_users WHERE username=@username AND passw=@passw AND ustatus=0 )
BEGIN
			SELECT 'User Account De-Active.' AS RES RETURN;
		END

-- USER LOCKED 
IF EXISTS ( SELECT 1 FROM tbl_users WHERE username=@username AND passw=@passw AND ustatus=1 AND IsLocked='Y')
BEGIN
			SELECT 'User is  Locked.' AS RES RETURN;
		END
-- PASSWORD CHECK
IF NOT EXISTS ( SELECT 1 FROM tbl_users WHERE username=@username AND passw=@passw AND ustatus=1)
	BEGIN
-- UPDATE INCORRECT LOGIN ATTEMPT
	UPDATE tbl_users SET loginCnt = ISNULL(loginCnt,0)  + 1 WHERE username=@username
			
-- IF INCORRECT LOGIN ATTEMPT MORE THAN 5 TIMES THEN LOCK USER ACCOUNT
   IF((SELECT loginCnt FROM tbl_users WHERE username=@username AND passw=@passw ) > 5 )
		BEGIN
				UPDATE tbl_users SET IsLocked = 'Y' WHERE username=@username AND passw=@passw
		END			
		SELECT 'PASSWORD INCORRECT.' AS RES
		END				
	ELSE 
		BEGIN
			select u.userid,username,passw,logintype,cu.compcode,c.cmpname ,address1,address2,gstin,panno,mobileno,email,website,
            footer1,footer2,footer3,footer4,footer5,RES=''
            from tbl_users as u
            left join tbl_compusers as cu on(u.userid=cu.userid)
            left join tbl_company as c on(cu.compcode=c.compcode)
            where username=@username and passw=@passw and ustatus=1
        END
 END
END
go
--------------------------------------------------------
alter procedure usp_login_tbl_users1
@username varchar(100),
@passw varchar(50)
as    
select u.userid,
username,
passw,
logintype,
cu.compcode,
c.cmpname ,
address1,
address2,
gstin,
panno,
mobileno,email,website,
footer1,footer2,footer3,footer4,footer5
from tbl_users as u
left join tbl_compusers as cu on(u.userid=cu.userid)
left join tbl_company as c on(cu.compcode=c.compcode)
where username=@username and passw=@passw and ustatus='Active'
go
--------------------------------------------------------
alter procedure usp_insert_tbl_users
@userid int,
@username varchar(100) ,
@passw varchar(50),
@logintype smallint,
@registered datetime,
@Latest_Activity datetime,
@ustatus bit,
@IsLocked char(1),
@loginCnt int
as
insert into tbl_users
(
userid,
username,
passw,
logintype,
registered,
Latest_Activity,
ustatus,
IsLocked,
loginCnt
)values
(
@userid,
@username,
@passw,
@logintype,
@registered,
@Latest_Activity,
@ustatus,
@IsLocked,
@loginCnt
)
go
--------------------------------------------------------
alter procedure usp_update_tbl_users
@userid int,
@username varchar(100) ,
@passw varchar(50),
@logintype smallint,
@registered datetime,
@Latest_Activity datetime,
@ustatus bit,
@IsLocked char(1),
@loginCnt int
as
update tbl_users
set
username=@username,
passw=@passw,
logintype=@logintype,
Latest_Activity=@Latest_Activity,
ustatus=@ustatus,
IsLocked=@IsLocked,
loginCnt=@loginCnt
where userid=@userid
go
--------------------------------------------------------
alter procedure usp_display_tbl_users
as
select * from tbl_users
go
--------------------------------------------------------
---spMenu_Permission
--------------------------------------------------------
alter procedure usp_insert_tbl_menu_permission
@compcode tinyint,
@userid int,
@mid int,
@menuper bit,--{(Yes/No)-(1/0)}--menu permission
@approve bit
as
insert into tbl_menu_permission
(
compcode,
userid,
mid,
menuper,
approve
)
values
(
@compcode,
@userid,
@mid,
@menuper,
@approve
)
go
--------------------------------------------------------
alter procedure usp_delete_permission
@userid int,
@compcode tinyint,
@mid int
as
delete from tbl_userpermission where compcode=@compcode and userid=@userid and mid=@mid
delete from tbl_menu_permission where compcode=@compcode and userid=@userid 
and mid=@mid
go
--------------------------------------------------------
---spUserPermission
--------------------------------------------------------
alter procedure usp_insert_tbl_userpermission
@compcode tinyint,
@userid int,
@mid int,
@smid int,
@addper bit,
@editper bit,
@deleteper bit,
@viewper bit,
@printper bit,
@menuper bit
as
insert into tbl_userpermission
(
compcode,
userid,
mid,
smid,
addper,
editper,
deleteper,
viewper,
printper,
menuper
)values
(
@compcode,
@userid,
@mid,
@smid,
@addper,
@editper,
@deleteper,
@viewper,
@printper,
@menuper
)
go
--------------------------------------------------------
---spCompany
--------------------------------------------------------
alter procedure usp_display_tbl_company
as
select cu.userid,c.* from tbl_compusers as cu
left join tbl_company as c on(cu.compcode=c.compcode)
go

-------------------------------------------------------
-----tbl_country
-------------------------------------------------------
alter procedure usp_insert_into_tbl_country
@cname char(50),
@cstatus char(2)
as
insert into tbl_country
(
cname,
cstatus
)
values
(
@cname,
@cstatus
)
go
-------------------------------------------------------
alter procedure usp_update_tbl_country
@cid int,
@cname char(50),
@cstatus char(2)
as
update tbl_country
set
cname=@cname,
cstatus=@cstatus
where cid=@cid
go
-------------------------------------------------------
-----tbl_state
-------------------------------------------------------
alter procedure usp_insert_into_tbl_state
@cid int,
@statename char(50),
@sstatus char(2)
as
insert into tbl_state
(
cid,
statename,
sstatus
)
values
(
@cid,
@statename,
@sstatus
)
go
-------------------------------------------------------
alter procedure usp_update_tbl_state
@sid int,
@cid int,
@statename char(50),
@sstatus char(2)
as
update tbl_state
set
cid=@cid,
statename=@statename,
sstatus=@sstatus
where sid=@sid
go
-------------------------------------------------------
-----tbl_city
-------------------------------------------------------
alter procedure usp_insert_into_tbl_city
@sid int ,
@cid int,
@cityname char(50),
@citystatus char(2)
as
insert into tbl_city
(
sid,
cid,
cityname,
citystatus
)
values
(
@sid,
@cid,
@cityname,
@citystatus
)
go
-------------------------------------------------------
alter procedure usp_update_tbl_city
@cityid int,
@sid int ,
@cid int,
@cityname char(50),
@citystatus char(2)
as
update tbl_city
set
sid=@sid,
cid=@cid,
cityname=@cityname,
citystatus=@citystatus
where cityid=@cityid
go
-------------------------------------------------------
-----tbl_partytype
-------------------------------------------------------
alter procedure usp_insert_tbl_partytype
@parttype char(50)
as
insert into tbl_partytype
values
(
@parttype
)
go
----------------------------------------------------------
alter procedure usp_update_tbl_partytype
@partytypeid int,
@parttype char(50)
as
update tbl_partytype
set 
parttype=@parttype
where partytypeid=@partytypeid
go
----------------------------------------------------------
alter procedure iusp_delete_from_tbl_partytype
@partytypeid int
as
delete from tbl_partytype
where partytypeid=@partytypeid
go
-----------------------------------------------------------
----tbl_membertype
-----------------------------------------------------------
alter procedure usp_insert_tbl_membertype
@membertype char(50)
as
insert into tbl_membertype
values
(
@membertype
)
go
-----------------------------------------------------------
alter procedure usp_update_tbl_membertype
@membertypeid int,
@membertype char(50)
as
update tbl_membertype
set membertype =@membertype 
where membertypeid=@membertypeid
go
-----------------------------------------------------------
alter procedure usp_delete_tbl_membertype
@membertypeid int
as
delete from tbl_membertype where membertypeid=@membertypeid
go
---------------------------------------------------------------------------
---spmagzine
---------------------------------------------------------------------------
alter procedure usp_insert_tbl_magzine
@mid int ,
@mcode nvarchar(50),
@mname nvarchar(100),
@mtype char(1),
@publishname nvarchar(100),
@isbn nvarchar(50),
@otherdetails nvarchar(100),
@mstatus char(1),
@cdate date,
@frequency char(1),
@gstper decimal(5,2)
as
insert into tbl_magzine
(
mid,
mcode,
mname,
mtype,
publishname,
isbn,
otherdetails,
mstatus ,
cdate,
frequency,
gstper
)
values
(
@mid,
@mcode,
@mname,
@mtype,
@publishname,
@isbn,
@otherdetails,
@mstatus ,
@cdate,
@frequency,
@gstper
)
go
---------------------------------------------------------------------------
alter procedure usp_update_tbl_magzine
@mid int ,
@mcode nvarchar(50),
@mname nvarchar(100),
@mtype char(1),
@publishname nvarchar(100),
@isbn nvarchar(50),
@otherdetails nvarchar(100),
@mstatus char(1),
@cdate date,
@frequency char(1),
@gstper decimal(5,2)
as
update tbl_magzine
set
mcode=@mcode,
mname=@mname,
mtype=@mtype,
publishname=@publishname,
isbn=@isbn,
otherdetails=@otherdetails,
mstatus=@mstatus,
frequency=@frequency,
gstper=@gstper
where mid=@mid
go
---------------------------------------------------------------------------
alter procedure usp_delete_tbl_magzine
@mid int
as 
delete from tbl_magzine_price_master  where mid = @mid
delete from tbl_magzine where mid=@mid
go
---------------------------------------------------------------------------
alter procedure usp_get_tbl_magzine
@mid int
as 
select * from tbl_magzine where mid=@mid
go
---------------------------------------------------------------------------
alter procedure usp_display_tbl_magzine_pagewise
@displaylength int,
@displayStart int,
@sortcol int,
@sortdir nvarchar(10)='asc',
@search nvarchar(255)=null
as 
begin
SELECT * INTO #Temp_tbl_magzine from tbl_magzine

declare @FirstRec int,@LastRec int
set @FirstRec=@displayStart;
set @LastRec=@displayStart+@displaylength;
with CTE_tbl_magzine as
(
select ROW_NUMBER() over (order by 
case when (@sortcol=1 and  @sortdir='asc')then mcode end asc,
case when (@sortcol=1 and  @sortdir='desc')then mcode end desc,

case when (@sortcol=2 and  @sortdir='asc')then  mname end asc,
case when (@sortcol=2 and  @sortdir='desc')then mname end desc
) as RowNum,
COUNT(*) over()as TotalCount, * from #Temp_tbl_magzine
where (@search is null 
or mcode like '%'+@search+'%'
or mname like '%'+@search+'%'
)
)
select * from CTE_tbl_magzine where RowNum>@FirstRec and RowNum<= @LastRec
DROP TABLE #Temp_tbl_magzine
end
go
---------------------------------------------------------------------------
---tbl_pricemaste
---------------------------------------------------------------------------
alter procedure insert_magzine_pricemaster
@mid int,
@tenure char(50),
@rate decimal(15,2),
@cdate date
as
insert into tbl_magzine_price_master
(
mid,
tenure ,
rate ,
cdate
)
values
(
@mid ,
@tenure ,
@rate ,
@cdate
)
go
---------------------------------------------------------------------------
alter procedure update_magzine_pricemaster
@priceid int,
@mid int,
@tenure char(50),
@rate decimal(15,2),
@cdate date
as
update tbl_magzine_price_master set
tenure =@tenure,
rate=@rate,
cdate=@cdate 
where mid = @mid and priceid =@priceid
go
---------------------------------------------------------------------------
alter procedure delete_magzine_pricemaster
@mid int
as 
delete from tbl_magzine_price_master  where mid = @mid
go
---------------------------------------------------------------------------
alter procedure delete_magzine_pricemaster_new
@mzid int,
@priceid int
as 
delete from tbl_magzine_price_master  where mid = @mzid and priceid=@priceid
go

---------------------------------------------------------------------------
alter procedure usp_get_magzine_pricemaster
@mid int
as 
select * from tbl_magzine_price_master  where mid = @mid
go
---------------------------------------------------------------------------
---spMagzineRelease
---------------------------------------------------------------------------
alter procedure usp_insert_tbl_magzine_release
@mrid int, -----------magzine Release id
@mid int,            -----------magzine ID
@mrtitle nvarchar(100), -----------magzine release title
@dtitle nvarchar(100),  -----------display title
@releasecode nvarchar(20),
@descrp nvarchar(max) ,
@pdf nvarchar(max),
@vieourl nvarchar(max),
@coverimg nvarchar(max),
@thumnail nvarchar(max),
@releasedate date,
@releasedmonth varchar(10),
@releaseyear varchar(4),
@ispublish bit,
@stock int
as
insert into tbl_magzine_release
(
mrid,
mid,
mrtitle,
dtitle,
releasecode,
descrp,
pdf,
vieourl,
coverimg,
thumnail,
releasedate,
releasedmonth,
releaseyear,
ispublish,
stock
)values
(
@mrid,
@mid,
@mrtitle,
@dtitle,
@releasecode,
@descrp,
@pdf,
@vieourl,
@coverimg,
@thumnail,
@releasedate,
@releasedmonth,
@releaseyear,
@ispublish,
@stock
)
go
---------------------------------------------------------------------------
alter procedure usp_update_tbl_magzine_release
@mrid int, -----------magzine Release id
@mid int,            -----------magzine ID
@mrtitle nvarchar(100), -----------magzine release title
@dtitle nvarchar(100),  -----------display title
@releasecode nvarchar(20),
@descrp nvarchar(max) ,
@pdf nvarchar(max),
@vieourl nvarchar(max),
@coverimg nvarchar(max),
@thumnail nvarchar(max),
@releasedate date,
@releasedmonth varchar(10),
@releaseyear varchar(4),
@ispublish bit,
@stock int
as
update tbl_magzine_release
set
mid=@mid,
mrtitle=@mrtitle,
dtitle=@dtitle,
releasecode=@releasecode,
descrp=@descrp,
pdf=@pdf,
vieourl=@vieourl,
coverimg=@coverimg,
thumnail=@thumnail,
releasedate=@releasedate,
releasedmonth=@releasedmonth,
releaseyear=@releaseyear,
ispublish=@ispublish,
stock=@stock
where mrid=@mrid
go
---------------------------------------------------------------------------
alter procedure usp_delete_tbl_magzine_release
@mrid int
as
delete from tbl_magzine_release where mrid=@mrid
go
---------------------------------------------------------------------------
alter procedure usp_get_tbl_magzine_release
@mrid int
as
select mr.*,m.mname from tbl_magzine_release as mr
left join tbl_magzine as m on(mr.mid=m.mid) 
where mrid=@mrid
go
---------------------------------------------------------------------------
alter procedure usp_display_tbl_magzine_release_pagewise
@displaylength int,
@displayStart int,
@sortcol int,
@sortdir nvarchar(10)='asc',
@search nvarchar(255)=null
as 
begin
SELECT mr.*,m.mname,m.mcode 
INTO #Temp_tbl_magzine_release 
from tbl_magzine_release as mr
left join tbl_magzine as m on(mr.mid=m.mid)

declare @FirstRec int,@LastRec int
set @FirstRec=@displayStart;
set @LastRec=@displayStart+@displaylength;
with CTE_tbl_magzine_release as
(
select ROW_NUMBER() over (order by 
case when (@sortcol=1 and  @sortdir='asc')then mcode end asc,
case when (@sortcol=1 and  @sortdir='desc')then mcode end desc,

case when (@sortcol=2 and  @sortdir='asc')then mname end asc,
case when (@sortcol=2 and  @sortdir='desc')then mname end desc,

case when (@sortcol=3 and  @sortdir='asc')then mrtitle end asc,
case when (@sortcol=3 and  @sortdir='desc')then mrtitle end desc,

case when (@sortcol=4 and  @sortdir='asc')then  releasecode end asc,
case when (@sortcol=4 and  @sortdir='desc')then releasecode end desc,

case when (@sortcol=5 and  @sortdir='asc')then  releasedmonth end asc,
case when (@sortcol=5 and  @sortdir='desc')then releasedmonth end desc,

case when (@sortcol=6 and  @sortdir='asc')then  stock end asc,
case when (@sortcol=6 and  @sortdir='desc')then stock end desc
) as RowNum,
COUNT(*) over()as TotalCount, * from #Temp_tbl_magzine_release
where (@search is null 
or mrtitle like '%'+@search+'%'
or releasecode like '%'+@search+'%'
or releasedmonth like '%'+@search+'%'
or stock like '%'+@search+'%'
or mcode like '%'+@search+'%'
or mname like '%'+@search+'%'
)
)
select * from CTE_tbl_magzine_release where RowNum>@FirstRec and RowNum<= @LastRec
DROP TABLE #Temp_tbl_magzine_release
end
go

---------------------------------------------------------------------------
---spMagzineArticles-------------------------------------------------------
---------------------------------------------------------------------------
alter procedure usp_insert_tbl_magzine_articles
@arid int  , -----------article Release id
@mrid int, -----------magzine Release id
@atitlecode nvarchar(100),-----------article code
@artitle nvarchar(100), -----------article title
@coverimg nvarchar(max),
@thumnail nvarchar(max),
@descrp nvarchar(max) ,
@arurl nvarchar(max) ,
@releasedate date,
@ispublish bit
as
declare @maid int -- magzine ID
set @maid = (select mid from tbl_magzine_release where mrid=@mrid)-- get magzine id behalf og mrid mz reled id
insert into tbl_magzine_articles
(
arid   , 
mid ,        
mrid ,
atitlecode ,
artitle ,
coverimg ,
thumnail ,
descrp  ,
arurl  ,
releasedate ,
ispublish 
)values
(
@arid   , 
@maid       ,
@mrid ,
@atitlecode ,
@artitle ,
@coverimg ,
@thumnail ,
@descrp  ,
@arurl  ,
@releasedate ,
@ispublish 
)
go
---------------------------------------------------------------------------
alter procedure usp_update_tbl_magzine_articles
@arid int  , -----------article Release id
@mrid int, -----------magzine Release id
@atitlecode nvarchar(100),-----------article code
@artitle nvarchar(100), -----------article title
@coverimg nvarchar(max),
@thumnail nvarchar(max),
@descrp nvarchar(max) ,
@arurl nvarchar(max) ,
@releasedate date,
@ispublish bit
as
declare @maid int -- magzine ID
set @maid = (select mid from tbl_magzine_release where mrid=@mrid)-- get magzine id behalf og mrid mz reled id
update tbl_magzine_articles
set
mid =@maid ,
mrid =@mrid,
atitlecode =@atitlecode,
artitle= @artitle ,
coverimg=@coverimg ,
thumnail=@thumnail ,
descrp=@descrp  ,
arurl=@arurl  ,
releasedate=@releasedate,
ispublish=@ispublish
where arid=@arid
go
---------------------------------------------------------------------------

alter procedure usp_delete_tbl_magzine_articles
@arid int
as
delete from tbl_magzine_articles where arid=@arid
go
---------------------------------------------------------------------------
alter procedure usp_delete_tbl_magzine_articles
@arid int
as
delete from tbl_magzine_articles where arid=@arid
go
---------------------------------------------------------------------------
alter procedure usp_get_relesedcode
@SearchText varchar(50)
as
select mrid,releasecode+','+dtitle  as title from tbl_magzine_release  where dtitle like '%'+ @SearchText + '%'
go

---------------------------------------------------------------------------

alter procedure usp_display_tbl_magzine_article_pagewise
@displaylength int,
@displayStart int,
@sortcol int,
@sortdir nvarchar(10)='asc',
@search nvarchar(255)=null,
@mrid int
as 
begin
select distinct arid,atitlecode, artitle,a.ispublish,releasecode,dtitle as mrtitle into #Temp_tbl_magzine_article
from tbl_magzine_articles as a left join tbl_magzine_release as r on (a.mrid = r.mrid)
where 1 =case when @mrid='0' or r.mrid=@mrid then 1 else 0  end

declare @FirstRec int,@LastRec int
set @FirstRec=@displayStart;
set @LastRec=@displayStart+@displaylength;

with CTE_tbl_magzine_article as
(
select ROW_NUMBER() over (order by 
case when (@sortcol=1 and  @sortdir='asc')then releasecode end asc,
case when (@sortcol=1 and  @sortdir='desc')then releasecode end desc,

case when (@sortcol=2 and  @sortdir='asc')then mrtitle end asc,
case when (@sortcol=2 and  @sortdir='desc')then mrtitle end desc,

case when (@sortcol=4 and  @sortdir='asc')then artitle end asc,
case when (@sortcol=4 and  @sortdir='desc')then artitle end desc,

case when (@sortcol=3 and  @sortdir='asc')then atitlecode end asc,
case when (@sortcol=3 and  @sortdir='desc')then atitlecode end desc

) as RowNum,
COUNT(*) over()as TotalCount, * from #Temp_tbl_magzine_article
where (@search is null 
or atitlecode like '%'+@search+'%'
or artitle like '%'+@search+'%'
or mrtitle like '%'+@search+'%'
or releasecode like '%'+@search+'%'
)
)
select * from CTE_tbl_magzine_article where RowNum>@FirstRec and RowNum<= @LastRec
DROP TABLE #Temp_tbl_magzine_article
end
go

----------------------------------------------------------------------------
alter procedure usp_get_tbl_magzine_article
@arid int
as
select distinct arid,atitlecode,artitle,a.ispublish, releasecode,mrtitle,r.mrid,a.coverimg,a.thumnail,a.descrp,a.arurl,a.ispublish,a.releasedate
from tbl_magzine_articles as a left join tbl_magzine_release as r on (a.mrid = r.mrid)
where a.arid=@arid
go

---------------------------------------------------------------------------
--spMember
---------------------------------------------------------------------------
alter procedure usp_get_membercode
@SearchText varchar(50)
as
select id,[name],mscode,mobile from tbl_membership where mscode+mobile+[name] like '%'+ @SearchText + '%' and partytype='m'
go
--------------------------------------------------------------------------
alter procedure usp_generate_mscode
AS
BEGIN
 DECLARE @NewEmpID VARCHAR(25);
 DECLARE @PreFix VARCHAR(10) = 'MR';
 DECLARE @Id INT;

 SELECT @Id = ISNULL(MAX(ID),0) + 1 FROM tbl_membership

 set @NewEmpID = @PreFix + RIGHT('0000000' + CAST(@Id AS VARCHAR(7)), 7)
 select @NewEmpID as nid
END
go

---------------------------------------------------------------------------
alter procedure usp_generate_partycode
AS
BEGIN
 DECLARE @NewEmpID VARCHAR(25);
 DECLARE @PreFix VARCHAR(10) = 'PR';
 DECLARE @Id INT;

 SELECT @Id = ISNULL(MAX(ID),0) + 1 FROM tbl_membership 

 set @NewEmpID = @PreFix + RIGHT('0000000' + CAST(@Id AS VARCHAR(7)), 7)
 select @NewEmpID as nid
END
go

---------------------------------------------------------------------------
alter procedure usp_insert_tbl_membership
@mscode nvarchar(10),
@name nvarchar(100),
@mobile nvarchar(10),
@WhatsAppNo nvarchar(10),
@dob nvarchar(10)=null,
@email nvarchar(30),
@education nvarchar(max) ,
@jobprofile nvarchar(max) ,
@gender char(1),
@age int=null,
@marital nvarchar(10),
@membertypeid int=null,
@isForwarder bit,
@parentid nvarchar(10),
@IsDeActive bit,
@discount decimal(10,2),
@partytype char(1),
@gstin char(30),
@contactperson char(100)

as
insert into tbl_membership
(
mscode,
name,
mobile,
WhatsAppNo,
dob,
email,
education,
jobprofile,
gender,
age,
marital,
membertypeid,
isForwarder,
parentid,
IsDeActive,
discount,
partytype ,
gstin ,
contactperson 
)values
(
@mscode,
@name,
@mobile,
@WhatsAppNo,
@dob,
@email,
@education,
@jobprofile,
@gender,
@age,
@marital,
@membertypeid,
@isForwarder,
@parentid,
@IsDeActive,
@discount,
@partytype ,
@gstin ,
@contactperson 
)
go
---------------------------------------------------------------------------
alter procedure usp_update_tbl_membership
@mscode nvarchar(10),
@name nvarchar(100),
@mobile nvarchar(10),
@WhatsAppNo nvarchar(10),
@dob nvarchar(10) =null,
@email nvarchar(30),
@education nvarchar(max) ,
@jobprofile nvarchar(max) ,
@gender char(1),
@age int,
@marital nvarchar(10),
@membertypeid int =null,
@isForwarder bit,
@parentid nvarchar(10),
@IsDeActive bit,
@discount decimal(10,2),
@partytype char(1),
@gstin char(30),
@contactperson char(100)
as
update tbl_membership
set
name=@name,
mobile=@mobile,
WhatsAppNo=@WhatsAppNo,
dob=@dob,
email=@email,
education=@education,
jobprofile=@jobprofile,
gender=@gender,
age=@age,
marital=@marital,
membertypeid=@membertypeid,
isForwarder=@isForwarder,
parentid=@parentid,
IsDeActive=@IsDeActive,
discount=@discount,
partytype=@partytype,
gstin=@gstin,
contactperson=@contactperson
where
mscode=@mscode
go
---------------------------------------------------------------------------
alter procedure usp_delete_tbl_membership
@mscode nvarchar(10)
as
delete from tbl_membership_address where mscode=@mscode
delete from tbl_membership where mscode=@mscode
go
---------------------------------------------------------------------------
alter procedure usp_get_tbl_membership
@mscode nvarchar(10)
as
select m1.*,mt.membertype,pincode,landkamr,localaddress,c1.cname,s.statename,ct.cityname,ar.areaname
from tbl_membership as m1
left join tbl_membership_address as ma on (m1.mscode = ma.mscode)
left join tbl_membertype as mt on (m1.membertypeid = mt.membertypeid)
left join tbl_country as c1 on (ma.countryid = c1.cid)
left join tbl_state as s on (ma.stateid = s.sid)
left join tbl_city as ct on (ma.cityid = ct.cityid)
left join tbl_area as ar on (ma.areaid = ar.areaid)
where m1.mscode=@mscode 
go
---------------------------------------------------------------------------
alter procedure usp_get_tbl_membership_child
@mscode nvarchar(10)
as
select m1.*,mt.membertype,pincode,landkamr,localaddress,c1.cname,s.statename,ct.cityname,ar.areaname
from tbl_membership as m1
left join tbl_membership_address as ma on (m1.mscode = ma.mscode)
left join tbl_membertype as mt on (m1.membertypeid = mt.membertypeid)
left join tbl_country as c1 on (ma.countryid = c1.cid)
left join tbl_state as s on (ma.stateid = s.sid)
left join tbl_city as ct on (ma.cityid = ct.cityid)
left join tbl_area as ar on (ma.areaid = ar.areaid)
where m1.parentid=@mscode
go
---------------------------------------------------------------------------
alter procedure usp_display_tbl_membership_pagewise
@displaylength int,
@displayStart int,
@sortcol int,
@sortdir nvarchar(10)='asc',
@search nvarchar(255)=null,
@cid int,
@sid int,
@cityid int,
@deactive bit,
@forworder bit,
@mtype int,
@gender char(1),
@ptype char(1)
as 
begin
select m1.*,mt.membertype,pincode,landkamr,localaddress,c1.cname,s.statename,ct.cityname,ar.areaname
into #Temp_tbl_membership
from tbl_membership as m1
left join tbl_membership_address as ma on (m1.mscode = ma.mscode)
left join tbl_membertype as mt on (m1.membertypeid = mt.membertypeid)
left join tbl_country as c1 on (ma.countryid = c1.cid)
left join tbl_state as s on (ma.stateid = s.sid)
left join tbl_city as ct on (ma.cityid = ct.cityid)
left join tbl_area as ar on (ma.areaid = ar.areaid)
where 
1=case when @cid=0 or ma.countryid=@cid then 1 else 0 end 
and 1=case when @sid=0 or ma.stateid=@sid then 1 else 0 end 
and 1=case when @cityid=0 or ma.cityid=@cityid then 1 else 0 end 
and 1=case when @deactive=0 or m1.IsDeActive=@deactive then 1 else 0 end 
and 1=case when @forworder=0 or m1.isForwarder=@forworder then 1 else 0 end 
and 1=case when @mtype=0 or m1.membertypeid=@mtype then 1 else 0 end 
and 1=case when @gender='0' or m1.gender=@gender then 1 else 0 end
and m1.partytype=@ptype

declare @FirstRec int,@LastRec int
set @FirstRec=@displayStart;
set @LastRec=@displayStart+@displaylength;

with CTE_tbl_membership as
(
select ROW_NUMBER() over (order by 
case when (@sortcol=1 and  @sortdir='asc')then mscode end asc,
case when (@sortcol=1 and  @sortdir='desc')then mscode end desc,

case when (@sortcol=2 and  @sortdir='asc')then name end asc,
case when (@sortcol=2 and  @sortdir='desc')then name end desc,

case when (@sortcol=3 and  @sortdir='asc')then mobile end asc,
case when (@sortcol=3 and  @sortdir='desc')then mobile end desc,

case when (@sortcol=4 and  @sortdir='asc')then gender end asc,
case when (@sortcol=4 and  @sortdir='desc')then gender end desc,

case when (@sortcol=5 and  @sortdir='asc')then membertype end asc,
case when (@sortcol=5 and  @sortdir='desc')then membertype end desc,

case when (@sortcol=6 and  @sortdir='asc')then discount end asc,
case when (@sortcol=6 and  @sortdir='desc')then discount end desc,

case when (@sortcol=7 and  @sortdir='asc')then IsDeActive end asc,
case when (@sortcol=7 and  @sortdir='desc')then IsDeActive end desc

) as RowNum,
COUNT(*) over()as TotalCount, * from #Temp_tbl_membership
where (@search is null 
or mscode like '%'+@search+'%'
or name like '%'+@search+'%'
or mobile like '%'+@search+'%'
or WhatsAppNo like '%'+@search+'%'
or age like '%'+@search+'%'
or membertype like '%'+@search+'%'
or pincode like '%'+@search+'%'
or landkamr like '%'+@search+'%'
or localaddress like '%'+@search+'%'
or cname like '%'+@search+'%'
or cityname like '%'+@search+'%'
or areaname like '%'+@search+'%'
)
)
select * from CTE_tbl_membership where RowNum>@FirstRec and RowNum<= @LastRec
DROP TABLE #Temp_tbl_membership
end
go
---------------------------------------------------------------------------
--spMemberAddress
---------------------------------------------------------------------------
alter procedure usp_insert_tbl_membership_address
@mscode nvarchar(10),
@countryid int,
@stateid int,
@cityid int,
@areaid int,
@pincode nvarchar(30),
@landkamr nvarchar(max) ,
@localaddress nvarchar(max) 
as
insert into tbl_membership_address
(
mscode,
countryid,
stateid,
cityid,
areaid,
pincode,
landkamr,
localaddress 
)values
(
@mscode,
@countryid,
@stateid,
@cityid,
@areaid,
@pincode,
@landkamr,
@localaddress 
)
go
---------------------------------------------------------------------------
alter procedure usp_update_tbl_membership_address
@mscode nvarchar(10),
@countryid int,
@stateid int,
@cityid int,
@areaid int,
@pincode nvarchar(30),
@landkamr nvarchar(max) ,
@localaddress nvarchar(max) 
as
update tbl_membership_address
set
countryid=@countryid,
stateid=@stateid,
cityid=@cityid,
areaid=@areaid,
pincode=@pincode,
landkamr=@landkamr,
localaddress=@localaddress
where mscode=@mscode
go
---------------------------------------------------------------------------
alter procedure usp_delete_tbl_membership_address
@mscode nvarchar(10)
as
delete from tbl_membership_address
where mscode=@mscode
go
---------------------------------------------------------------------------
alter procedure usp_get_tbl_membership_address
@mscode nvarchar(10)
as
select ma.*,c.cname,s.statename,ct.cityname,a.areaname
from tbl_membership_address as ma
left join tbl_country as c on(ma.countryid=c.cid)
left join tbl_state as s on(ma.stateid=s.sid)
left join tbl_city as ct on(ma.cityid=ct.cityid)
left join tbl_area as a on(ma.areaid=a.areaid)
where mscode=@mscode
go
---------------------------------------------------------------------------
--Subscriptions-----
---------------------------------------------------------------------------
alter procedure usp_insert_tbl_subscriptions_master_detials_subscriber
@sbid int,
@msid int , --membeshipid
@mscode nvarchar(10), --membershipcode 
@mzid int, -- magzine id
@mzname nvarchar(max),--magzine name
@mfrequency varchar(100), --magzine freeqquency
@mtenure varchar(100), -- magzine tenure
@substatus char(1),
@expmonth char(15),
@expdate  date,
@fromdate  date,
@expyear char(5),
@gperiods int
as
insert into tbl_subscriptions_master_detials_subscriber
(
sbid,
msid  ,
mscode ,
mzid ,
mzname ,
mfrequency ,
mtenure ,
substatus ,
expmonth,
expdate,
fromdate,
expyear,
gperiods 
)
values
(
@sbid,
@msid  ,
@mscode ,
@mzid ,
@mzname ,
@mfrequency ,
@mtenure ,
@substatus ,
@expmonth,
@expdate,
@fromdate,
@expyear,
@gperiods
)
go
-----------------------------------------------------------------------------------
alter procedure usp_insert_subscriptions
@sbid int,
@msid int ,
@mscode nvarchar(10),
@mzid int,
@mzname nvarchar(max),
@mfrequency varchar(100),
@mtenure varchar(100),
@pricemasterid int,
@fromdate date,
@todate date,
@greaseperiods char(3),
@rate  decimal(15,2),
@amt  decimal(15,2),
@gst decimal(15,2),
@gstamt decimal(15,2),
@dis decimal(15,2),
@disamt decimal(15,2),
@paybleamt decimal(15,2),
@substatus char(1)
as
insert into tbl_subscriptions
(
sbid,
msid  ,
mscode ,
mzid ,
mzname ,
mfrequency ,
mtenure ,
pricemasterid ,
fromdate ,
todate ,
greaseperiods ,
rate  ,
amt  ,
gst ,
gstamt ,
dis ,
disamt,
paybleamt ,
substatus 
)
values
(
@sbid,
@msid  ,
@mscode ,
@mzid ,
@mzname ,
@mfrequency ,
@mtenure ,
@pricemasterid ,
@fromdate ,
@todate ,
@greaseperiods ,
@rate  ,
@amt  ,
@gst ,
@gstamt ,
@dis ,
@disamt,
@paybleamt ,
@substatus 
)
go
-----------------------------------------------------------------------------------
alter procedure usp_update_tbl_subscriptions_master_detials_subscriber
@sbid int,
@msid int ,
@mzid int, --
@mfrequency varchar(100), 
@mtenure varchar(100), 
@substatus char(1),
@expmonth char(15),
@expdate  date,
@fromdate  date,
@expyear char(5),
@gperiods int
as
update tbl_subscriptions_master_detials_subscriber set
mfrequency =@mfrequency, 
mtenure =@mtenure, 
substatus=@substatus ,
expmonth =@expmonth,
expdate  =@expdate,
fromdate =@fromdate ,
expyear =@expyear ,
gperiods= @gperiods
where sbid=@sbid and msid=@msid and mzid=@mzid
go

---------------------------------------------------------------------------------

alter procedure usp_update_tbl_subscriptions_master_detials_subscriber_grace
@sbid int,
@msid int ,
@mzid int, 
@gperiods int
as
update tbl_subscriptions_master_detials_subscriber set
gperiods= @gperiods
where sbid=@sbid and msid=@msid and mzid=@mzid
go

---------------------------------------------------------------------------------
alter procedure usp_subscriptions_delete
@sbid int
as
delete from tbl_subscriptions where sbid=@sbid
go
---------------------------------------------------------------------------------
alter procedure usp_update_subscriptions
@sbid int,
@msid int ,
@mscode nvarchar(10),
@mzid int,
@mzname nvarchar(max),
@mfrequency varchar(100),
@mtenure varchar(100),
@pricemasterid int,
@fromdate date,
@todate date,
@greaseperiods char(3),
@rate  decimal(15,2),
@amt  decimal(15,2),
@gst decimal(15,2),
@gstamt decimal(15,2),
@dis decimal(15,2),
@disamt decimal(15,2),
@paybleamt decimal(15,2),
@substatus char(1)
as
update tbl_tbl_subscriptions set
msid = @msid,
mscode =@mscode ,
mzid =@mzid,
mzname =@mzname ,
mfrequency =@mfrequency ,
mtenure=@mtenure ,
pricemasterid =@pricemasterid,
fromdate=@fromdate ,
todate=@todate ,
greaseperiods=@greaseperiods ,
rate=@rate ,
amt=@amt,
gst=@gst,
gstamt=@gstamt ,
dis =@dis,
disamt=@disamt,
paybleamt =@paybleamt,
substatus =@substatus
go
------------------------------------------------------------------------------
--alter procedure usp_display_tbl_subscriptions_pagewise
--@displaylength int,
--@displayStart int,
--@sortcol int,
--@sortdir nvarchar(10)='asc',
--@search nvarchar(255)=null,
--@cid int,
--@sid int,
--@cityid int,
--@deactive bit,
--@forworder bit,
--@mtype int,
--@gender char(1),
--@mzid int,
--@tenure char(1),
--@fromdate date,
--@todate date
--as 
--begin
--select distinct sbid,m.mscode ,m.name,m.mobile,mzname,mfrequency,mtenure,fromdate,dateadd(dd,CAST(greaseperiods as int),todate) as todate, datename(month, todate) as expmonth,greaseperiods,sp.rate, dis, paybleamt,substatus
--into #Temp_tbl_subscriptions
--from tbl_membership as m
--right join tbl_subscriptions as sp on (m.id = sp.msid)
--left join tbl_membership_address as ma on (m.mscode = ma.mscode)
--left join tbl_membertype as mt on (m.membertypeid = mt.membertypeid)
--left join tbl_country as c1 on (ma.countryid = c1.cid)
--left join tbl_state as s on (ma.stateid = s.sid)
--left join tbl_city as ct on (ma.cityid = ct.cityid)
--left join tbl_area as ar on (ma.areaid = ar.areaid)
--left join tbl_magzine_price_master as p on (sp.mzid = p.mid)

--where 
--(1=case when @cid=0 or ma.countryid=@cid then 1 else 0 end 
--and 1=case when @sid=0 or ma.stateid=@sid then 1 else 0 end 
--and 1=case when @cityid=0 or ma.cityid=@cityid then 1 else 0 end 
--and 1=case when @deactive=0 or m.IsDeActive=@deactive then 1 else 0 end 
--and 1=case when @mtype=0 or m.membertypeid=@mtype then 1 else 0 end 
--and 1=case when @gender='0' or m.gender=@gender then 1 else 0 end
--and 1=case when @mzid='0' or sp.mzid=@mzid then 1 else 0 end
--and 1=case when @tenure='0' or sp.mtenure=@tenure then 1 else 0 end)
--and  fromdate >=@fromdate and todate <=@todate 

--declare @FirstRec int,@LastRec int
--set @FirstRec=@displayStart;
--set @LastRec=@displayStart+@displaylength;

--with CTE_tbl_subscriptions as
--(
--select ROW_NUMBER() over (order by
--case when (@sortcol=1 and  @sortdir='asc')then [name] end asc,
--case when (@sortcol=1 and  @sortdir='desc')then [name] end desc,

--case when (@sortcol=2 and  @sortdir='asc')then mzname end asc,
--case when (@sortcol=2 and  @sortdir='desc')then mzname end desc

----case when (@sortcol=3 and  @sortdir='asc')then mfrequency end asc,
----case when (@sortcol=3 and  @sortdir='desc')then mfrequency end desc,

----case when (@sortcol=4 and  @sortdir='asc')then mtenure end asc,
----case when (@sortcol=4 and  @sortdir='desc')then mtenure end desc,

----case when (@sortcol=5 and  @sortdir='asc')then fromdate end asc,
----case when (@sortcol=5 and  @sortdir='desc')then fromdate end desc,

----case when (@sortcol=6 and  @sortdir='asc')then todate end asc,
----case when (@sortcol=6 and  @sortdir='desc')then todate end desc,

----case when (@sortcol=7 and  @sortdir='asc')then expmonth end asc,
----case when (@sortcol=7 and  @sortdir='desc')then expmonth end desc,

----case when (@sortcol=8 and  @sortdir='asc')then greaseperiods end asc,
----case when (@sortcol=8 and  @sortdir='desc')then greaseperiods end desc,

----case when (@sortcol=9 and  @sortdir='asc')then rate end asc,
----case when (@sortcol=9 and  @sortdir='desc')then rate end desc,

----case when (@sortcol=10 and  @sortdir='asc')then dis end asc,
----case when (@sortcol=10 and  @sortdir='desc')then dis end desc,

----case when (@sortcol=11 and  @sortdir='asc')then paybleamt end asc,
----case when (@sortcol=11 and  @sortdir='desc')then paybleamt end desc,

----case when (@sortcol=12 and  @sortdir='asc')then substatus end asc,
----case when (@sortcol=12 and  @sortdir='desc')then substatus end desc



--) as RowNum,
--COUNT(*) over()as TotalCount, * from #Temp_tbl_subscriptions
--where (@search is null 
--or mscode like '%'+@search+'%'
--or name like '%'+@search+'%'
--or mzname like '%'+@search+'%'
--or expmonth like '%'+@search+'%'
--)
--)
--select * from CTE_tbl_subscriptions where RowNum>@FirstRec and RowNum<= @LastRec
--DROP TABLE #Temp_tbl_subscriptions
--end
--go
--new


alter procedure usp_display_tbl_subscriptions_pagewise
@displaylength int,
@displayStart int,
@sortcol int,
@sortdir nvarchar(10)='asc',
@search nvarchar(255)=null,
@cid int,
@sid int,
@cityid int,
@deactive bit,
@forworder bit,
@mtype int,
@gender char(1),
@mzid int,
@tenure char(1),
@fromdate date,
@todate date,
@expmonth char(10),
@expyear char(5)
as 
begin
select distinct sd.sbid,sd.mzid,sd.msid,m.mscode ,m.name,m.mobile,sd.mzname,sd.mfrequency,sd.mtenure,sd.fromdate,dateadd(dd,CAST(gperiods as int),expdate) as todate, datename(month, dateadd(dd,CAST(gperiods as int),expdate)) as expmonth,
sd.gperiods,sd.substatus,sd.expmonth as expiremonth,sd.expyear
into #Temp_tbl_subscriptions
from tbl_subscriptions_master_detials_subscriber as sd
left join tbl_membership as m on(m.id = sd.msid and m.partytype='m')
left join tbl_membership_address as ma on (m.mscode = ma.mscode)
left join tbl_membertype as mt on (m.membertypeid = mt.membertypeid)
left join tbl_country as c1 on (ma.countryid = c1.cid)
left join tbl_state as s on (ma.stateid = s.sid)
left join tbl_city as ct on (ma.cityid = ct.cityid)
left join tbl_area as ar on (ma.areaid = ar.areaid)
left join tbl_magzine_price_master as p on (sd.mzid = p.mid)

where 
(1=case when @cid=0 or ma.countryid=@cid then 1 else 0 end 
and 1=case when @sid=0 or ma.stateid=@sid then 1 else 0 end 
and 1=case when @cityid=0 or ma.cityid=@cityid then 1 else 0 end 
and 1=case when @deactive=0 or m.IsDeActive=@deactive then 1 else 0 end 
and 1=case when @mtype=0 or m.membertypeid=@mtype then 1 else 0 end 
and 1=case when @gender='0' or m.gender=@gender then 1 else 0 end
and 1=case when @mzid='0' or sd.mzid=@mzid then 1 else 0 end
and 1=case when @tenure='0' or sd.mtenure=@tenure then 1 else 0 end
and 1= case when @expmonth='0' or datename(month, dateadd(dd,CAST(gperiods as int),expdate))=@expmonth then 1 else 0 end
and 1 = case when @expyear='0' or sd.expyear=@expyear then 1 else 0 end
)
and  sd.fromdate >=@fromdate and expdate <=@todate 

declare @FirstRec int,@LastRec int
set @FirstRec=@displayStart;
set @LastRec=@displayStart+@displaylength;

with CTE_tbl_subscriptions as
(
select ROW_NUMBER() over (order by
case when (@sortcol=1 and  @sortdir='asc')then [name] end asc,
case when (@sortcol=1 and  @sortdir='desc')then [name] end desc,

case when (@sortcol=2 and  @sortdir='asc')then mzname end asc,
case when (@sortcol=2 and  @sortdir='desc')then mzname end desc

--case when (@sortcol=3 and  @sortdir='asc')then mfrequency end asc,
--case when (@sortcol=3 and  @sortdir='desc')then mfrequency end desc,

--case when (@sortcol=4 and  @sortdir='asc')then mtenure end asc,
--case when (@sortcol=4 and  @sortdir='desc')then mtenure end desc,

--case when (@sortcol=5 and  @sortdir='asc')then fromdate end asc,
--case when (@sortcol=5 and  @sortdir='desc')then fromdate end desc,

--case when (@sortcol=6 and  @sortdir='asc')then todate end asc,
--case when (@sortcol=6 and  @sortdir='desc')then todate end desc,

--case when (@sortcol=7 and  @sortdir='asc')then expmonth end asc,
--case when (@sortcol=7 and  @sortdir='desc')then expmonth end desc,

--case when (@sortcol=8 and  @sortdir='asc')then greaseperiods end asc,
--case when (@sortcol=8 and  @sortdir='desc')then greaseperiods end desc,

--case when (@sortcol=9 and  @sortdir='asc')then rate end asc,
--case when (@sortcol=9 and  @sortdir='desc')then rate end desc,

--case when (@sortcol=10 and  @sortdir='asc')then dis end asc,
--case when (@sortcol=10 and  @sortdir='desc')then dis end desc,

--case when (@sortcol=11 and  @sortdir='asc')then paybleamt end asc,
--case when (@sortcol=11 and  @sortdir='desc')then paybleamt end desc,

--case when (@sortcol=12 and  @sortdir='asc')then substatus end asc,
--case when (@sortcol=12 and  @sortdir='desc')then substatus end desc



) as RowNum,
COUNT(*) over()as TotalCount, * from #Temp_tbl_subscriptions
where (@search is null 
or mscode like '%'+@search+'%'
or name like '%'+@search+'%'
or mzname like '%'+@search+'%'
or expmonth like '%'+@search+'%'
)
)
select * from CTE_tbl_subscriptions where RowNum>@FirstRec and RowNum<= @LastRec
DROP TABLE #Temp_tbl_subscriptions
end
go
----------------------------------------------------------
--sss
alter procedure usp_getrecordse_fromsubscription_by_sbid
@sbid  int,
@mzid int,
@msid int
as
select s.sbidlid,a.sbid,s.msid,s.mzid,s.mzname,a.mscode,m.name,m.mobile,s.mfrequency,s.mtenure,s.substatus,s.fromdate,dateadd(dd,CAST(greaseperiods as int),s.todate) as todate,a.gperiods
from tbl_subscriptions_master_detials_subscriber as a left join tbl_subscriptions as s on (a.sbid=s.sbid)
left join tbl_membership as m on (a.msid=m.id and  m.partytype='m')
where s.sbid=@sbid and s.mzid=@mzid and s.msid=@msid
order by s.sbidlid asc
go

---------------------------------------------------------------

alter procedure usp_display_tbl_subscriptions_dispatch
@search nvarchar(255)=null,
@cid int,
@sid int,
@cityid int,
@area int,
@deactive bit,
@mtype int,
@gender char(1),
@mzid int,
@mrid int,
@gperiods int,
@ctstatus char(1)
as
select *  from vw_sbscriber
where 
1=case when @cid=0 or cid=@cid then 1 else 0 end 
and 1=case when @sid=0 or sid=@sid then 1 else 0 end 
and 1=case when @cityid=0 or cityid=@cityid then 1 else 0 end 
and 1=case when @area=0 or areaid=@area then 1 else 0 end 
and 1=case when @deactive=0 or substatus=@deactive then 1 else 0 end 
and 1=case when @mtype=0 or membertypeid=@mtype then 1 else 0 end 
and 1=case when @gender='0' or gender=@gender then 1 else 0 end
and 1=case when @mzid='0' or mzid=@mzid then 1 else 0 end
and 1=case when @mrid='0' or mrid=@mrid then 1 else 0 end
and 1=case when @gperiods='0' or gperiods=@gperiods then 1 else 0 end
and 1=case when @ctstatus='0' or cstatus=@ctstatus then 1 else 0 end
go

--------------------------------------------------------------------------------------
alter view vw_sbscriber
as
select s.sbid,s.msid,s.mscode,s.mzid,s.fromdate,s.expdate,s.gperiods,s.expmonth,s.expyear,s.substatus,m.name,mt.membertype,ma.localaddress,m.gender,dateadd(dd,CAST(gperiods as int),expdate) as expdatewithgrace,datename(month, dateadd(dd,CAST(gperiods as int),expdate)) as expmonthwithgrace,
cstatus =(case when   CAST( GETDATE() AS Date) <=dateadd(dd,CAST(gperiods as int),expdate) and gperiods=0 then 'A'
else 
case when   CAST( GETDATE() AS Date) <=dateadd(dd,CAST(gperiods as int),expdate) and gperiods<>0 then 'G' else 'E' end
end) ,
c.cid,st.sid,ct.cityid,ar.areaid,mg.mid,ml.mrid,mt.membertypeid,m.mobile
from tbl_subscriptions_master_detials_subscriber as s
left join tbl_membership as m on(s.msid=m.id and m.partytype='m')
left join tbl_membership_address as ma  on (m.mscode = ma.mscode)
left join tbl_membertype as mt on (m.membertypeid = mt.membertypeid)
left join tbl_country as c on (ma.countryid=c.cid)
left join tbl_state as st on (ma.stateid=st.sid)
left join tbl_city as ct on (ma.cityid=ct.cityid)
left join tbl_area as ar on (ma.areaid=ar.areaid)
left join tbl_magzine as mg  on (s.mzid=mg.mid)
left join tbl_magzine_release as ml on (mg.mid = ml.mrid)

go
----------------------------------------------------------------------------------
alter procedure usp_get_stock_mrelease
@mrid int
as
select stock from tbl_magzine_release where mrid=@mrid and ispublish =1
go
----------------------------------------------------------------------------------
alter procedure usp_generate_orderno
AS
BEGIN
 DECLARE @NewEmpID VARCHAR(25);
 DECLARE @PreFix VARCHAR(10) = 'OR';
 DECLARE @Id INT;

 SELECT @Id = ISNULL(MAX(lvno),0) + 1 FROM tbl_master_ledger

 set @NewEmpID = @PreFix + RIGHT('0000000' + CAST(@Id AS VARCHAR(7)), 7)
 select @NewEmpID as nid
END
go
---------------------------------------------------------------------------------------------------------------------------------
alter procedure usp_insert_tbl_master
@vno int,
@vdate date,
@vtype char(2),
@msid int, 
@mscode nvarchar(10),
@mzid int,
@mzname nvarchar(200),
@mrid int,
@mrname nvarchar(250),
@cid int,
@sit int,
@cityid int,
@areaid int,
@mtypeid int =null,
@gender char(1),
@selecttype char(1),
@price decimal(15,0),
@rate decimal(15,0),
@qty decimal(15,0),
@gstper decimal(15,0),
@gstamt decimal(15,0),
@disper decimal(15,0),
@disamt decimal(15,0),
@paybleamt decimal(15,0),
@totalamt decimal(15,0),
@trantype char(2),-- transaction status
@partyid int
as

insert into tbl_master
(
vno ,
vdate,
vtype ,
msid , 
mscode ,
mzid ,
mzname, 
mrid ,
mrname, 
cid ,
sit ,
cityid, 
areaid ,
mtypeid ,
gender ,
selecttype, 
price ,
rate ,
qty ,
gstper, 
gstamt ,
disper ,
disamt ,
paybleamt, 
totalamt,
trantype,
partyid
)
values
(
@vno ,
@vdate,
@vtype ,
@msid , 
@mscode,
@mzid ,
@mzname, 
@mrid ,
@mrname, 
@cid ,
@sit ,
@cityid, 
@areaid ,
@mtypeid ,
@gender ,
@selecttype, 
@price ,
@rate ,
@qty ,
@gstper, 
@gstamt ,
@disper ,
@disamt ,
@paybleamt, 
@totalamt,
@trantype,
@partyid
)
go
-------------------------------------------------------
alter procedure usp_update_tbl_master
@vno int,
@vdate date,
@vtype char(2),
@totalamt decimal(15,0),
@trantype char(2),
@partyid int
as
update tbl_master
set
vdate=@vdate,
trantype=@trantype,
partyid=@partyid,
totalamt= @totalamt
where vno=@vno and vtype=@vtype
go
-------------------------------------------------------
alter procedure usp_insert_tbl_master_ledger
@ordeno nvarchar(10),
@vno int,
@lvdate date,
@lvtype char(2),
@msid int,
@mscode nvarchar(10) =null,
@selecttype char(1),
@mzid int,
@mzname nvarchar(200),
@mrid int,
@mrname nvarchar(250),
@price decimal(15,0),
@rate decimal(15,0),
@qty decimal(15,0),
@gstper decimal(15,0),
@gstamt decimal(15,0),
@disper decimal(15,0),
@disamt decimal(15,0),
@paybleamt decimal(15,0),
@totalamt decimal(15,0),
@lstatsu char(1), -- 0 means not dispatch,
@trantype char(2)-- transaction status
as
insert into tbl_master_ledger
(
ordeno ,
vno ,
lvdate ,
lvtype ,
msid ,
mscode ,
selecttype ,
mzid ,
mzname ,
mrid ,
mrname ,
price ,
rate ,
qty ,
gstper ,
gstamt ,
disper ,
disamt ,
paybleamt ,
totalamt ,
lstatsu ,
trantype
)
values
(

@ordeno ,
@vno ,
@lvdate ,
@lvtype ,
@msid ,
@mscode ,
@selecttype ,
@mzid ,
@mzname ,
@mrid ,
@mrname ,
@price ,
@rate ,
@qty ,
@gstper ,
@gstamt ,
@disper ,
@disamt ,
@paybleamt ,
@totalamt ,
@lstatsu ,
@trantype
)
go
----------------------------------------------------------------------------------
alter procedure  usp_delete_tbl_master_ledger
@vno int,
@vtype char(2)
as
delete tbl_master_ledger where  vno=@vno and lvtype=@vtype
go

-----------------------------------------------------------------------------------
alter procedure usp_display_tbl_subscriptions_dispatch_list
@search nvarchar(255)=null,
@cid int,
@sid int,
@cityid int,
@area int,
@deactive bit,
@mtype int,
@gender char(1),
@mzid int,
@mrid int,
@ctstatus char(1)
as
select *  from tbl_master
where 
1=case when @cid=0 or cid=@cid then 1 else 0 end 
and 1=case when @sid=0 or sit=@sid then 1 else 0 end 
and 1=case when @cityid=0 or cityid=@cityid then 1 else 0 end 
and 1=case when @area=0 or areaid=@area then 1 else 0 end 
and 1=case when @mtype=0 or mtypeid=@mtype then 1 else 0 end 
and 1=case when @gender='0' or gender=@gender then 1 else 0 end
and 1=case when @mzid='0' or mzid=@mzid then 1 else 0 end
and 1=case when @mrid='0' or mrid=@mrid then 1 else 0 end
and 1=case when @ctstatus='0' or selecttype=@ctstatus then 1 else 0 end
and vtype='Di'
go
-----------------------------------------------------------------------------

alter procedure usp_delete_tbl_master
@vno int,
@vtype char(2)
as
delete from tbl_master_ledger where vno=@vno and lvtype=@vtype
delete from tbl_master where vno=@vno and vtype=@vtype
go

-------------------------------------------------------------------------------------------------------------------------------------------------------
alter procedure usp_display_tbl_subscriptions_dispatch_printlabels
@search nvarchar(255)=null,
@cid int,
@sid int,
@cityid int,
@area int,
@deactive bit,
@mtype int,
@gender char(1),
@mzid int,
@mrid int,
@gperiods int,
@ctstatus char(1)
as
select *  from vw_sbscriber_printlables
where 
1=case when @cid=0 or cid=@cid then 1 else 0 end 
and 1=case when @sid=0 or sid=@sid then 1 else 0 end 
and 1=case when @cityid=0 or cityid=@cityid then 1 else 0 end 
and 1=case when @area=0 or areaid=@area then 1 else 0 end 
and 1=case when @deactive=0 or substatus=@deactive then 1 else 0 end 
and 1=case when @mtype=0 or membertypeid=@mtype then 1 else 0 end 
and 1=case when @gender='0' or gender=@gender then 1 else 0 end
and 1=case when @mzid='0' or mzid=@mzid then 1 else 0 end
and 1=case when @mrid='0' or mrid=@mrid then 1 else 0 end
and 1=case when @gperiods='0' or gperiods=@gperiods then 1 else 0 end
and 1=case when @ctstatus='0' or cstatus=@ctstatus then 1 else 0 end
go


--------------------------------------------------------------------------------------------------------------------------------------------------------
alter view vw_sbscriber_printlables
as
select s.sbid,s.msid,s.mscode,s.mzid,s.fromdate,s.expdate,s.gperiods,s.expmonth,s.expyear,s.substatus,m.name,mt.membertype,m.gender,dateadd(dd,CAST(gperiods as int),expdate) as expdatewithgrace,datename(month, dateadd(dd,CAST(gperiods as int),expdate)) as expmonthwithgrace,
cstatus =(case when   CAST( GETDATE() AS Date) <=dateadd(dd,CAST(gperiods as int),expdate) and gperiods=0 then 'A'
else 
case when   CAST( GETDATE() AS Date) <=dateadd(dd,CAST(gperiods as int),expdate) and gperiods<>0 then 'G' else 'E' end
end) ,
c.cid,st.sid,ct.cityid,ar.areaid,mg.mid,ml.mrid,mt.membertypeid,m.mobile,m.isForwarder,
ma.localaddress+'_'+pincode+'_'+LTRIM(RTRIM(c.cname))+'_'+LTRIM(RTRIM(st.statename))+'_'+LTRIM(RTRIM(ct.cityname))+'_'+LTRIM(RTRIM(ar.areaname))+'_'+name+'_'+mobile as localaddress,
forwaderaddress = case when m.isForwarder=0 then isnull((select localaddress+'_'+pincode+'_'+(select top 1 LTRIM(RTRIM(cname)) from tbl_country where cid=st.countryid)+'_'+(select top 1 LTRIM(RTRIM(statename)) from tbl_state where sid=st.stateid)+'_'+(select top 1 LTRIM(RTRIM(cityname)) from tbl_city where cityid=st.cityid)+'_'+(select top 1 LTRIM(RTRIM(areaname)) from tbl_area where areaid=st.areaid)+'_'+(select top 1 name from tbl_membership where mscode=st.mscode)+'_'+(select top 1 mobile from tbl_membership where mscode=st.mscode)  from tbl_membership_address as st where mscode = m.parentid ),ma.localaddress+'_'+pincode+'_'+LTRIM(RTRIM(c.cname))+'_'+LTRIM(RTRIM(st.statename))+'_'+LTRIM(RTRIM(ct.cityname))+'_'+LTRIM(RTRIM(ar.areaname))+'_'+name+'_'+mobile) 
else ma.localaddress+'_'+pincode+'_'+LTRIM(RTRIM(c.cname))+'_'+LTRIM(RTRIM(st.statename))+'_'+LTRIM(RTRIM(ct.cityname))+'_'+LTRIM(RTRIM(ar.areaname))+'_'+name+'_'+mobile end

from tbl_subscriptions_master_detials_subscriber as s
left join tbl_membership as m on(s.msid=m.id and m.partytype='m')
left join tbl_membership_address as ma  on (m.mscode = ma.mscode)
left join tbl_membertype as mt on (m.membertypeid = mt.membertypeid)
left join tbl_country as c on (ma.countryid=c.cid)
left join tbl_state as st on (ma.stateid=st.sid)
left join tbl_city as ct on (ma.cityid=ct.cityid)
left join tbl_area as ar on (ma.areaid=ar.areaid)
left join tbl_magzine as mg  on (s.mzid=mg.mid)
left join tbl_magzine_release as ml on (mg.mid = ml.mrid)
go

--------------------------------------------------------------------------------------
alter procedure insert_into_tbl_print
@pvno int,
@ordeno nvarchar(10),
@lvno int,
@vno int,
@pdate date,
@vtype char(2),
@mzid  int,
@mzname nvarchar(max),
@mrid int,
@mrname nvarchar(max),
@msid int,
@localaddress  nvarchar(max),
@isfarwarder bit,
@isfarwarderid varchar(10),
@isfarwarderstatus bit,
@addresstosend nvarchar(max),
@printqty int
as
 
insert into tbl_print
(
pvno ,
ordeno ,
lvno ,
vno ,
pdate ,
vtype ,
mzid  ,
mzname ,
mrid ,
mrname ,
msid ,
localaddress  ,
isfarwarder ,
isfarwarderid,
isfarwarderstatus,
addresstosend ,
printqty 
)
values
(
@pvno ,
@ordeno ,
@lvno ,
@vno ,
@pdate ,
@vtype ,
@mzid  ,
@mzname ,
@mrid ,
@mrname ,
@msid ,
@localaddress  ,
@isfarwarder ,
@isfarwarderid,
@isfarwarderstatus,
@addresstosend ,
@printqty 

)
go

----------------------------------------------------------------------------------
alter procedure usp_delete_print
@vno int,
@vtype char(2)
as
delete from tbl_print where vno=@vno  and vtype=@vtype
go

--------------------------------------------------------------------------------------

alter procedure usp_display_tbl_print_pagewise
@displaylength int,
@displayStart int,
@sortcol int,
@sortdir nvarchar(10)='asc',
@search nvarchar(255)=null,
@mzid int,
@mrid int
as 
begin
select distinct vno, pvno,mzname,mrname,pdate,count(lvno) as printcount into #Temp_tbl_print
from tbl_print 
where 1 =case when @mzid='0' or mzid=@mzid then 1 else 0  end and
     1 =case when @mrid='0' or mrid=@mrid then 1 else 0  end
group by vno, pvno,mzname,mrname,pdate


declare @FirstRec int,@LastRec int
set @FirstRec=@displayStart;
set @LastRec=@displayStart+@displaylength;

with CTE_tbl_print as
(
select ROW_NUMBER() over (order by 
case when (@sortcol=1 and  @sortdir='asc')then vno end asc,
case when (@sortcol=1 and  @sortdir='desc')then vno end desc,

case when (@sortcol=2 and  @sortdir='asc')then pvno end asc,
case when (@sortcol=2 and  @sortdir='desc')then pvno end desc,

case when (@sortcol=5 and  @sortdir='asc')then pdate end asc,
case when (@sortcol=5 and  @sortdir='desc')then pdate end desc

) as RowNum,
COUNT(*) over()as TotalCount, * from #Temp_tbl_print
where (@search is null 
or pvno like '%'+@search+'%'
or vno like '%'+@search+'%'
or mzname like '%'+@search+'%'
or mrname like '%'+@search+'%'
)
)
select * from CTE_tbl_print where RowNum>@FirstRec and RowNum<= @LastRec
DROP TABLE #Temp_tbl_print
end
go
--------------------------------------------------
alter procedure usp_delete_tbl_print_records
@vno int,
@pvno int,
@vtype char(2)
as
delete from tbl_print where vno=@vno and pvno=@pvno and vtype='Pt'
go
----------------------------------------------------------------------------
ALTER procedure usp_display_tbl_subscriptions_dispatch_list_print
@vno int,
@vtype char(2),
@isfarwarder bit
as
select  l.msid,l.mzid,l.mzname,l.mrid,l.mrname,l.lvno,l.vno,l.ordeno,b.mscode,b.name,b.mobile,b.isForwarder,a.localaddress+'_'+pincode+'_'+LTRIM(RTRIM(c.cname))+'_'+LTRIM(RTRIM(s.statename))+'_'+LTRIM(RTRIM(y.cityname))+'_'+LTRIM(RTRIM(r.areaname))+'_'+name+'_'+mobile as localaddress,

forwaderaddress = case when @isfarwarder=1 then isnull((select localaddress+'_'+pincode+'_'+(select top 1 LTRIM(RTRIM(cname)) from tbl_country where cid=st.countryid)+'_'+(select top 1 LTRIM(RTRIM(statename)) from tbl_state where sid=st.stateid)+'_'+(select top 1 LTRIM(RTRIM(cityname)) from tbl_city where cityid=st.cityid)+'_'+(select top 1 LTRIM(RTRIM(areaname)) from tbl_area where areaid=st.areaid)+'_'+(select top 1 name from tbl_membership where mscode=st.mscode)+'_'+(select top 1 mobile from tbl_membership where mscode=st.mscode)  from tbl_membership_address as st where mscode = b.parentid ),a.localaddress+'_'+pincode+'_'+LTRIM(RTRIM(c.cname))+'_'+LTRIM(RTRIM(s.statename))+'_'+LTRIM(RTRIM(y.cityname))+'_'+LTRIM(RTRIM(r.areaname))+'_'+name+'_'+mobile) 
else a.localaddress+'_'+pincode+'_'+LTRIM(RTRIM(c.cname))+'_'+LTRIM(RTRIM(s.statename))+'_'+LTRIM(RTRIM(y.cityname))+'_'+LTRIM(RTRIM(r.areaname))+'_'+name+'_'+mobile end,
b.parentid,
printcount = ( select count(lvno) from tbl_print as lp where l.lvno =lp.lvno )
from tbl_master as m 
left join tbl_master_ledger as l on (m.vno=l.vno) 
left join tbl_membership as b on (l.msid=b.id)
left join tbl_membership_address as a on(b.mscode = a.mscode)
left join tbl_country as c on (a.countryid=c.cid)
left join tbl_state as s on(a.stateid=s.sid)
left join tbl_city as y on (a.cityid=y.cityid)
left join tbl_area as r on (a.areaid=r.areaid)
where
m.vno = @vno and m.vtype=@vtype and l.lvtype=@vtype
go

----------------------------------------------------------------------------
alter procedure usp_display_tbl_subscriptions_dispatch_list_print_dispatch
@vno int,
@vtype char(2)
as
select distinct l.lvno,l.vno,b.name,b.mobile,l.ordeno,
printstatus = case when l.lvno not in (select lvno from tbl_print as t where l.lvno=t.lvno ) then 0 else 1  end,
pid = isnull((select top 1 pid from tbl_print as y where l.lvno=y.lvno order by y.pid desc ),0),
pvno = isnull((select top 1 pvno from tbl_print as y where l.lvno=y.lvno order by y.pid desc ),0),
ISNULL(p.mzid,0)as mzid,isnull(p.mzname,0)as mzname,isnull(p.mrid,0) as mrid,isnull(p.mrname,0) as mrname,isnull(p.msid,0) as msid,
dispatchmod =isnull((select top 1 dispatchmod from tbl_dispatch as dp where l.lvno=dp.lvno order by dp.did desc ),0),
dispatchremarks =isnull((select top 1 dispatchremarks from tbl_dispatch as dp where l.lvno=dp.lvno order by dp.did desc ),0),
deldate =isnull((select top 1 deldate from tbl_dispatch as dp where l.lvno=dp.lvno order by dp.did desc ),'1900-01-01'),
dstatusremakrs =isnull((select top 1 dstatusremakrs from tbl_dispatch as dp where l.lvno=dp.lvno order by dp.did desc ),0),
dispatchstatus =isnull((select top 1 dispatchstatus from tbl_dispatch as dp where l.lvno=dp.lvno order by dp.did desc ),0)
from tbl_master_ledger as l
left join tbl_print as p on (l.vno=p.vno and l.lvno=p.lvno )
left join tbl_membership as b on (l.msid=b.id and b.partytype='m')
left join tbl_dispatch as d on(p.pid =d.pid)
left join tbl_membership_address as a on(b.mscode = a.mscode)
left join tbl_country as c on (a.countryid=c.cid)
left join tbl_state as s on(a.stateid=s.sid)
left join tbl_city as y on (a.cityid=y.cityid)
left join tbl_area as r on (a.areaid=r.areaid)
where l.vno =@vno and l.lvtype=@vtype
order by l.ordeno
go
----------------------------------------------------------------------------
alter procedure usp_insert_tbl_dispatch
@dvno int=null,
@ordeno nvarchar(10)=null,
@ddate date=null,
@vno int=null,
@lvno int =null,
@pvno int =null,
@pid  int =null,
@vtype char(2) =null,
@mzid int =null,
@mzname nvarchar(200)=null,
@mrid int =null,
@mrname nvarchar(250)=null,
@msid int =null,
@dispatchmod nvarchar(max) =null,
@dispatchremarks nvarchar(max)=null,
@deldate date =null,
@dstatusremakrs nvarchar(max)=null,
@dispatchstatus char(1)=null
as
insert into tbl_dispatch
(
dvno ,
ordeno ,
ddate ,
vno ,
lvno ,
pvno ,
pid  ,
vtype ,
mzid ,
mzname ,
mrid ,
mrname ,
msid ,
dispatchmod ,
dispatchremarks ,
deldate ,
dstatusremakrs ,
dispatchstatus 
)
values
(
@dvno ,
@ordeno ,
@ddate ,
@vno ,
@lvno ,
@pvno ,
@pid  ,
@vtype ,
@mzid ,
@mzname ,
@mrid ,
@mrname ,
@msid ,
@dispatchmod ,
@dispatchremarks ,
@deldate ,
@dstatusremakrs ,
@dispatchstatus 
)
go
------------------------------------------------------------------------------------------
alter procedure  usp_check_avlstock
@mzid int,
@mrid int
as
select distinct  (select sum(qty) from tbl_master_ledger where lvtype  in('Si', 'Sr', 'Sn')  ) - isnull((select sum(qty) from tbl_master_ledger where lvtype in('Sl','Da','Di')),0)  as stock 
from tbl_master_ledger where  mzid=@mzid and mrid=@mrid
go

---------------------------------------------------------
-----Sale---------
--------------------------------------------------------
alter procedure usp_display_tbl_sale_pagewise
@displaylength int,
@displayStart int,
@sortcol int,
@sortdir nvarchar(10)='asc',
@search nvarchar(255)=null,
@vtype char(2)
as 
begin
select  vno,vdate,name,totalamt  INTO #Temp_tbl_sale
from tbl_master as m left join tbl_membership as s on(m.partyid = s.id and partytype='p') 
where vtype=@vtype


declare @FirstRec int,@LastRec int
set @FirstRec=@displayStart;
set @LastRec=@displayStart+@displaylength;
with CTE_tbl_sale as
(
select ROW_NUMBER() over (order by 
case when (@sortcol=1 and  @sortdir='asc')then vno end asc,
case when (@sortcol=1 and  @sortdir='desc')then vno end desc,

case when (@sortcol=2 and  @sortdir='asc')then  vdate end asc,
case when (@sortcol=2 and  @sortdir='desc')then vdate end desc,

case when (@sortcol=3 and  @sortdir='asc')then  name end asc,
case when (@sortcol=3 and  @sortdir='desc')then name end desc,

case when (@sortcol=3 and  @sortdir='asc')then  totalamt end asc,
case when (@sortcol=3 and  @sortdir='desc')then totalamt end desc

) as RowNum,
COUNT(*) over()as TotalCount, * from #Temp_tbl_sale
where (@search is null 
or name like '%'+@search+'%'
or vno like '%'+@search+'%'
)
)
select * from CTE_tbl_sale where RowNum>@FirstRec and RowNum<= @LastRec
DROP TABLE #Temp_tbl_sale
end
go
----------------------------------------------------------------------------
alter procedure usp_get_sale_record_edit
@vno int,
@vtype char(2)
as
select *,ddl =cast(msid as nvarchar(10))+'-'+mscode from tbl_master_ledger where vno=@vno and lvtype=@vtype
go
---------------------------------------------------------------------------
---------------------------------------------------------------------------




--delete from tbl_magzine_release
--delete from tbl_magzine_articles
--delete from tbl_master
--delete from tbl_master_ledger
--delete from tbl_print
--delete from tbl_dispatch

--delete from tbl_subscriptions
--delete from tbl_subscriptions_master_detials_subscriber



select * from tbl_master
select * from tbl_master_ledger

select * from tbl_print
select * from tbl_dispatch


select s.name,m.vno,m.lvtype,r.vdate,sum(m.qty) from tbl_master as r
left join tbl_membership as s on (r.msid=s.id and r.mscode=s.mscode)
left join tbl_master as r on (m.vno = r.vno)
where vtype in('Sl', 'Di')
group by   s.name,m.vno,m.lvtype,r.vdate

