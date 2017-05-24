from ChatterUserInfoes in db.ChatterUserInfoes
select new {
  ChatterUserInfoes.AspNetUser.UserName,
  ChatterUserInfoes.Message,
  ChatterUserInfoes.Timestamp
}