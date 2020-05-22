use tweety

go

-- --TO VIEW COMMENTS ON A TWEET-- --
go
create procedure comments_of_a_tweet
	@tweet_id int
as
begin
	if @tweet_id in (select tweetID from tweets)
	begin
		select comment,name as username,c.date,c.time
		from (tweets t join comments c on t.tweetID=c.tweetID)join [user] u on commenterID=u.userID
		where t.tweetID=@tweet_id
	end
	else
	begin
		print('The tweet does not exists')
	end
end
go
-- --executing code-- --
execute comments_of_a_tweet
	@tweet_id=9

-- --TO VIEW NO. OF FOLLOWERS OF A USER-- --
go
create procedure no_of_followers
	@username varchar(30)
as
begin
	if @username in (select name from [user])
	begin
		select name as username,count(followerID) as followers
		from [user] u left join follower f on u.userID=f.userID
		where @username=name
		group by name
	end
	else
	begin
		print('There exists no user with this username')
	end
end
go
-- --executing code-- --
execute no_of_followers
	@username='mike_99'

-- --TO VIEW LIKERS OF A TWEET-- --
go
create procedure likers_of_a_tweet
	@tweet_id int
as
begin
	if @tweet_id in (select tweetID from tweets)
	begin
		if @tweet_id in (select tweetID from likes)
		begin
			select u.name as liked_by_users
			from (tweets t join likes l on t.tweetID=l.tweetID)join [user] u on l.likerID=u.userID
			where t.tweetID=@tweet_id
		end
		else
		begin
			print('not liked by anyone yet')
		end
	end
	else
	begin
		print('The tweet does not exists')
	end
end
go
-- --executing code-- --
execute likers_of_a_tweet
	@tweet_id=7

-- --TO VIEW DISLIKERS OF A TWEET-- --
go
create procedure dislikers_of_a_tweet
	@tweet_id int
as
begin
	if @tweet_id in (select tweetID from tweets)
	begin
		if @tweet_id in (select tweetID from dislikes)
		begin
			select u.name as disliked_by_users
			from (tweets t join dislikes d on t.tweetID=d.tweetID)join [user] u on d.dislikerID=u.userID
			where t.tweetID=@tweet_id
		end
		else
		begin
			print('not disliked by anyone yet')
		end
	end
	else
	begin
		print('The tweet does not exists')
	end
end
go
-- --executing code-- --
execute dislikers_of_a_tweet
	@tweet_id=2

-- --TO FOLLOW A USER-- --
go
create procedure follow
	@username varchar(30),@other_username varchar(30)
as
begin
	declare @id1 int,@id2 int
	if @username in (select name from [user]) and @other_username in (select name from [user])
	begin
		if @username!=@other_username
		begin
			select @id1=[userID]
			from [user]
			where name=@username

			select @id2=[userID]
			from [user]
			where name=@other_username

			if @id1 in (select followerID from follower where [userID]=@id2)
			begin
				print @username+(' is already following ')+@other_username
			end
			else
			begin
				insert into follower values(@id2,@id1)
				print @username+(' now following user ')+@other_username
			end
		end
		else
		begin
			print('cannot follow yourself')
		end
	end
	else
	begin
		print ('There is no user with this username')
	end
end
go
-- --executing code-- --
execute follow
	@username='ali_33',@other_username='ahmad_54'

-- --TO UNFOLLOW A USER-- --
go
create procedure unfollow
	@username varchar(30),@other_username varchar(30)
as
begin
	declare @id1 int,@id2 int
	if @username in (select name from [user]) and @other_username in (select name from [user])
	begin
		select @id1=[userID]
		from [user]
		where name=@username

		select @id2=[userID]
		from [user]
		where name=@other_username

		if @id1 in (select followerID from follower where [userID]=@id2)
		begin
			delete from follower where [userID]=@id2 and followerID=@id1
			print @username+(' is no longer following user ')+@other_username
		end
		else
		begin
			print @username+(' is not a follower of ')+@other_username+(' so cannot unfollow')
		end
	end
	else
	begin
		print ('There is no user with this username')
	end
end
go
-- --executing code-- --
execute unfollow
	@username='ali_33',@other_username='ahmad_54'

-- --TO CHANGE USERNAME-- --
go
create procedure change_username
	@old varchar(30),@new varchar(30),@password varchar(30)
as
begin
	if @old in(select name from [user])
	begin
		if @password=(select [password] from [user] where name=@old)
		begin
			if @new in(select name from [user])
			begin
				print ('username ')+@new+(' is not available')
			end
			else
			begin
				update [user] set name=@new where name=@old
				print('username changed from ')+@old+(' to ')+@new
			end
		end
		else
		begin
			print('wrong password')
		end
	end
	else
	begin
		print ('There is no user with this user name')
	end
end
go
-- --executing code-- --
execute change_username
	@old='ali',@new='ali_33',@password='p1234'

-- --TO CHANGE PASSWORD-- --
go
create procedure change_password
	@name varchar(30),@new varchar(30),@password varchar(30)
as
begin
	if @name in(select name from [user])
	begin
		if @password=(select [password] from [user] where name=@name)
		begin
			update [user] set [password]=@new where name=@name
			print('password changed from ')+@password+(' to ')+@new+(' for ')+@name
		end
		else
		begin
			print('wrong password')
		end
	end
	else
	begin
		print ('There is no user with this user name')
	end
end
go
-- --executing code-- --
execute change_password
	@name='ali_33',@new='p123',@password='p1234'

-- --TO CHANGE FIRST NAME-- --
go
create procedure change_first_name
	@username varchar(30),@new varchar(30),@password varchar(30)
as
begin
	if @username in(select name from [user])
	begin
		if @password=(select [password] from [user] where name=@username)
		begin
			declare @id int
			select @id=[userID] from [user] where name=@username
			if @id in(select userID from [profile])
			begin
				update [profile] set fname=@new where [userID]=@id
				print('first name changed to ')+@new+(' for ')+@username
			end
			else
			begin
				print @username+(' does not have a profile yet')
			end
		end
		else
		begin
			print('wrong password')
		end
	end
	else
	begin
		print ('There is no user with this user name')
	end
end
go
-- --executing code-- --
execute change_first_name
	@username='ali_33',@new='Ajmal',@password='p1234'

execute change_first_name
	@username='ahmad_54',@new='Ajmal',@password='p1239'
--select * from [user] u join [profile] p on u.userID=p.userID

-- --TO CHANGE LAST NAME-- --
go
create procedure change_last_name
	@username varchar(30),@new varchar(30),@password varchar(30)
as
begin
	if @username in(select name from [user])
	begin
		if @password=(select [password] from [user] where name=@username)
		begin
			declare @id int
			select @id=[userID] from [user] where name=@username
			if @id in(select userID from [profile])
			begin
				update [profile] set lname=@new where [userID]=@id
				print('last name changed to ')+@new+(' for ')+@username
			end
			else
			begin
				print @username+(' does not have a profile yet')
			end
		end
		else
		begin
			print('wrong password')
		end
	end
	else
	begin
		print ('There is no user with this user name')
	end
end
go
-- --executing code-- --
execute change_last_name
	@username='ali_33',@new='Ajmal',@password='p1234'
--select * from [user] u join [profile] p on u.userID=p.userID

-- --TO CHANGE GENDER-- --
go
create procedure change_gender
	@username varchar(30),@new char,@password varchar(30)
as
begin
	if @username in(select name from [user])
	begin
		if @password=(select [password] from [user] where name=@username)
		begin
			declare @id int
			select @id=[userID] from [user] where name=@username
			if(@new='M' or @new='F')
			begin
				if @id in(select [userID] from [profile])
				begin
					update [profile] set gender=@new where [userID]=@id
					print ('Gender changed to ')+@new+(' for ')+@username
				end
				else
				begin
					print @username+(' does not have a profile yet')
				end
			end
			else
			begin
				print('wrong input for gender')
			end
		end
		else
		begin
			print('wrong password')
		end
	end
	else
	begin
		print ('There is no user with this user name')
	end
end
go
-- --executing code-- --
execute change_gender
	@username='ahmad_54',@new='M',@password='p1239'
--select * from [user] u join [profile] p on u.userID=p.userID

-- --TO CHANGE EMAIL-- --
go
create procedure change_email
	@username varchar(30),@new varchar(30),@password varchar(30)
as
begin
	if @username in(select name from [user])
	begin
		if @password=(select [password] from [user] where name=@username)
		begin
			declare @id int
			select @id=[userID] from [user] where name=@username

			if @id in(select [userID] from [profile])
			begin
				update [profile] set email=@new where [userID]=@id
				print ('Email changed to ')+@new+(' for ')+@username
			end
			else
			begin
				print @username+(' does not have a profile yet')
			end
		end
		else
		begin
			print('wrong password')
		end
	end
	else
	begin
		print ('There is no user with this user name')
	end
end
go
-- --executing code-- --
execute change_email
	@username='ali_33',@new='ali.khan@gmail.com',@password='p1234'

-- --TO CHANGE COUNTRY-- --
go
create procedure change_country
	@username varchar(30),@new varchar(30),@password varchar(30)
as
begin
	if @username in(select name from [user])
	begin
		if @password=(select [password] from [user] where name=@username)
		begin
			declare @id int
			select @id=[userID] from [user] where name=@username

			if @id in(select [userID] from [profile])
			begin
				update [profile] set country=@new where [userID]=@id
				print ('Country changed to ')+@new+(' for ')+@username
			end
			else
			begin
				print @username+(' does not have a profile yet')
			end
		end
		else
		begin
			print('wrong password')
		end
	end
	else
	begin
		print ('There is no user with this user name')
	end
end
go
-- --executing code-- --
execute change_country
	@username='ali_33',@new='USA',@password='p1234'

-- --TO CHANGE STATUS-- --
go
create procedure change_status
	@username varchar(30),@new varchar(120),@password varchar(30)
as
begin
	if @username in(select name from [user])
	begin
		if @password=(select [password] from [user] where name=@username)
		begin
			declare @id int
			select @id=[userID] from [user] where name=@username

			if @id in(select [userID] from [profile])
			begin
				update [profile] set status=@new where [userID]=@id
				print ('Status changed to "')+@new+('" for ')+@username
			end
			else
			begin
				print @username+(' does not have a profile yet')
			end
		end
		else
		begin
			print('wrong password')
		end
	end
	else
	begin
		print ('There is no user with this user name')
	end
end
go
-- --executing code-- --
execute change_status
	@username='ali_33',@new='inactive these days',@password='p1234'

-- --TO LIKE A TWEET-- --
go
create procedure like_a_tweet
	@tweet_id int,@liker varchar(30)
as
begin
	if @tweet_id in(select tweetID from tweets)
	begin
		if @liker in(select name from [user])
		begin
			declare @id int
			select @id=userID from[user] where name=@liker
			if @id in(select likerID from likes where tweetID=@tweet_id)
			begin
				print ('This tweet is already liked by ')+@liker
			end
			else
			begin
				insert into likes values(@id,@tweet_id)
				print @liker+(' liked this tweet')
			end
		end
		else
		begin
			print('This user does not exists')
		end
	end
	else
	begin
		print('The tweet does not exists')
	end
end
go
-- --executing code-- --
execute like_a_tweet
	@tweet_id=1,@liker='mike_99'
--select * from likes

-- --TO UNLIKE A TWEET-- --
go
create procedure unlike_a_tweet
	@tweet_id int,@liker varchar(30)
as
begin
	if @tweet_id in(select tweetID from tweets)
	begin
		if @liker in(select name from [user])
		begin
			declare @id int
			select @id=userID from[user] where name=@liker
			if @id not in(select likerID from likes where tweetID=@tweet_id)
			begin
				print ('This tweet has not been liked by ')+@liker+(' so cannot unlike')
			end
			else
			begin
				delete from likes where tweetID=@tweet_id and likerID=@id
				print @liker+(' unliked this tweet')
			end
		end
		else
		begin
			print('This user does not exists')
		end
	end
	else
	begin
		print('The tweet does not exists')
	end
end
go
-- --executing code-- --
execute unlike_a_tweet
	@tweet_id=1,@liker='mike_99'
--select * from likes

-- --TO DISLIKE A TWEET-- --
go
create procedure dislike_a_tweet
	@tweet_id int,@disliker varchar(30)
as
begin
	if @tweet_id in(select tweetID from tweets)
	begin
		if @disliker in(select name from [user])
		begin
			declare @id int
			select @id=userID from[user] where name=@disliker
			if @id in(select dislikerID from dislikes where tweetID=@tweet_id)
			begin
				print ('This tweet is already disliked by ')+@disliker
			end
			else
			begin
				insert into dislikes values(@id,@tweet_id)
				print @disliker+(' disliked this tweet')
			end
		end
		else
		begin
			print('This user does not exists')
		end
	end
	else
	begin
		print('The tweet does not exists')
	end
end
go
-- --executing code-- --
execute dislike_a_tweet
	@tweet_id=2,@disliker='mike_99'
--select * from dislikes

-- --TO UNDISLIKE A TWEET-- --
go
create procedure undislike_a_tweet
	@tweet_id int,@disliker varchar(30)
as
begin
	if @tweet_id in(select tweetID from tweets)
	begin
		if @disliker in(select name from [user])
		begin
			declare @id int
			select @id=userID from[user] where name=@disliker
			if @id not in(select dislikerID from dislikes where tweetID=@tweet_id)
			begin
				print ('This tweet has not been disliked by ')+@disliker+(' so cannot undislike')
			end
			else
			begin
				delete from dislikes where tweetID=@tweet_id and dislikerID=@id
				print @disliker+(' undisliked this tweet')
			end
		end
		else
		begin
			print('This user does not exists')
		end
	end
	else
	begin
		print('The tweet does not exists')
	end
end
go
-- --executing code-- --
execute undislike_a_tweet
	@tweet_id=2,@disliker='mike_99'
--select * from dislikes

-- --TO POST A COMMENT-- --
go
create procedure comment
	@tweet_id int,@username varchar(30),@comment varchar(280)
as
begin
	if @tweet_id in(select tweetID from tweets)
	begin
		if @username in(select name from [user])
		begin
			declare @uid int,@id int
			select @uid=userID from[user] where name=@username
			select @id=max(commentID) from comments
			set @id=@id+1

			insert into comments values(@id,@tweet_id,@uid,@comment,convert(date,getdate()),convert(time,getdate()))
			print('comment posted')
		end
		else
		begin
			print('This user does not exists')
		end
	end
	else
	begin
		print('The tweet does not exists')
	end
end
go
-- --executing code-- --
execute comment
	@tweet_id=1,@username='ahmad_54',@comment='one can see it for sure'
--select * from comments

-- --TO REMOVE A FOLLOWER-- --
go
create procedure remove_follower
	@username varchar(30),@follower varchar(30)--,@password varchar(30)
as
begin
	--if @username in(select name from [user]) and @follower in(select name from [user])
	--begin
		--if @password=(select password from [user] where name=@username)
		--begin
			declare @uid int,@fid int
			select @uid=userID from [user] where name=@username
			select @fid=userID from [user] where name=@follower

			if @fid in(select followerID from follower where userID=@uid)
			begin
				delete from follower where userID=@uid and followerID=@fid
				print @username+(' removed the follower ')+@follower
			end
			else
			begin
				print @follower+(' is already not your follower')
			end
		--end
		--else
		--begin
			--print('wrong password')
		--end
	end
	else
	begin
		print ('There is no user with this username')
	end
end
go
-- --executing code-- --
execute remove_follower
	@username='ali_33',@follower='ahmad_54',@password='p1234'
--select * from follower

-- --TO CREATE A PROFILE-- --
go
create procedure make_profile
	@username varchar(30),@fname varchar(30),@password varchar(30)
as
begin
	if @username in(select name from [user])
	begin
		if @password=(select password from [user] where name=@username)
		begin
			declare @uid int
			select @uid=userID from [user] where name=@username
			if @uid in(select userID from [profile])
			begin
				print @username+(' already has a profile')
			end
			else
			begin
				insert into [profile](userID,fname) values(@uid,@fname)
				print('profile created for ')+@username
			end
		end
		else
		begin
			print('wrong password')
		end
	end
	else
	begin
		print ('There is no user with this username')
	end
end
go
-- --executing code-- --
execute make_profile
	@username='ali_33',@fname='Ali',@password='p1234'

-- --SEARCH A TWEET-- --
go
create procedure search_tweet
	@text varchar(140)
as
begin
	select t.tweet,u.name as username,t.date,t.time
	from tweets t join [user] u  on t.userID=u.userID
	where CHARINDEX(@text,tweet) != 0
end
go
-- --executing code-- --
execute search_tweet
	@text='turned tables'

-- --TO POST A TWEET-- --
go
create procedure tweet
	@username varchar(30),@tweet varchar(280)
as
begin
	if @username in (select name from [user])
	begin
		declare @uid int,@tid int
		select @uid=userID from [user] where name=@username
		select @tid=max(tweetID) from tweets
		set @tid=@tid+1

		insert into tweets values(@tid,@uid,@tweet,convert(date,getdate()),convert(time,getdate()))
		print('tweet posted')
	end
	else
	begin
		print('There is no user with this username')
	end
end
go
-- --executing code-- --
execute tweet
	@username='ali_33',@tweet='I got a promotion #life'
--select * from tweets

-- --TO LOGIN-- --
go
create procedure user_login
	@username varchar(30),@password varchar(30),@output int output
as
begin
	if @username in(select name from [user])
	begin
		if @password=(select [password] from [user] where name=@username)
		begin
			set @output=1
		end
		else
		begin
			set @output=0
		end
	end
	else
	begin
		set @output=0
	end
end
go
-- --executing code-- --
declare @result int
execute user_login
	@username='ali_33',@password='p123',@output=@result output
select @result
go
-- --TO VIEW A USER-- --
go
create procedure view_user
	@username varchar(30)
as
begin
		select name,password,displayPic,fname,lname,gender,DOB,email,country,status
		from [user] u left join [profile] p on u.userID=p.userID
		where name=@username
end
-- --executing code-- --
execute view_user
	@username='ahmad_54'

-- --TO SIGNUP-- --
go
create procedure user_signup
	@username varchar(30),@password varchar(30),@fname varchar(30),@lname varchar(30),@email varchar(30),
	@country varchar(30),@gender char,@output int output
as
begin
	if @username in(select name from [user])
	begin
		set @output=0
	end
	else
	begin
		declare @uid int
		select @uid=max(userID) from [user]
		set @uid=@uid+1
		select @uid

		insert into [user](userID,name,password) values(@uid,@username,@password)
		insert into [profile](userID,fname,lname,gender,email,country)values(@uid,@fname,@lname,@gender,@email,@country)

		set @output=1
	end
end

--TRENDING HASHTAGS---


go
create procedure trending_hashtag
as
begin

select top 5 hashtag.hashtag,count(hashtag.hashtag) as Count_#
		
from  hashtag join tweets on hashtag.tweetID=tweets.tweetID
		
where tweets.[date] <= GETDATE() and (DATEPART(m, tweets.[date]) >= DATEPART(m, DATEADD(m, -2, getdate())) )
	
group by hashtag.hashtag
		
order by count(hashtag.hashtag) desc
end


-------executing code-- --

execute trending_hashtag






-----People u should follow----

go

create procedure People_U_Should_Follow
	@username varchar(30)

as

begin

select distinct u2.name,u2.displayPic, p1.fname,p1.lname
	
from ((([user] as u join follower as f1 on u.userID=f1.userID)join follower as f2  on f1.followerID=f2.userID)join [user] as u2 on f2.followerID=u2.userID)join [profile] as p1 on f2.followerID=p1.userID
	
where u.name=@username and u.userID!=f2.followerID  and f2.followerID not in
(select  f11.followerID
	from ([user] as u1 join follower as f11 on u1.userID=f11.userID)
	
where  u1.name=@username)   

end

-- --executing code-- --

execute People_U_Should_Follow
	@username='sara_89'

-- --TO VIEW NO. OF FOLLOWING OF A USER-- --
go
create procedure no_of_followings
	@username varchar(30)
as
begin
	if @username in (select name from [user])
	begin
		select name as username,count(f.userID) as [following]
		from [user] u left join follower f on u.userID=f.followerID
		where @username=name
		group by name
	end
	else
	begin
		print('There exists no user with this username')
	end
end
go
-- --executing code-- --
execute no_of_followings
	@username='sara_89'

-- --TO GET FOLLOWING OF A USER-- --
go
create procedure followings
	@username varchar(30)
as
begin
	if @username in (select name from [user])
	begin
		select u.name as [following],u.displayPic,fname,lname,country,[status],gender,u1.name
		from (([user] u join follower f on u.userID=f.userID)join [user] u1 on u1.userID=followerID)join [profile] on u.userID=[profile].userID
		where u1.name=@username		
	end
	else
	begin
		print('There exists no user with this username')
	end
end
go
-- --executing code-- --
execute followings
	@username='sara_89'

-- --TO GET FOLLOWERS OF A USER-- --
go
create procedure followers
	@username varchar(30)
as
begin
	if @username in (select name from [user])
	begin
		select u.name,u1.name as follower,u1.displayPic,fname,lname,country,[status],gender
		from (([user] u join follower f on u.userID=f.userID)join [user] u1 on u1.userID=followerID)left join [profile] on u1.userID=[profile].userID
		where u.name=@username	
	end
	else
	begin
		print('There exists no user with this username')
	end
end
go
-- --executing code-- --
execute followers
	@username='sara_89'

