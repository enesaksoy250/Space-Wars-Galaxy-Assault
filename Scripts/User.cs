

public class User
{

    public string username;
    public  string email;
    public int gameTime;
    public int totalDestruction;
    public int totalLoss;
    public int onlineWins;

      public User(string username,string email,int gameTime,int totalDestruction,int totalLoss,int onlineWins)
      {

        this.username = username;
        this.email= email;
        this.gameTime = gameTime;
        this.totalDestruction = totalDestruction;
        this.totalLoss = totalLoss;
        this.onlineWins = onlineWins;

      }


}


