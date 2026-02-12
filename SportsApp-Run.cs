using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace SportsApp{
    public class SportsApp{
        List<Team> teams = new List<Team>();
        List<Match> matches = new List<Match>();

        public void Run(){
            LoadData();
            int choice;
            while(true)
            {
                Console.WriteLine("\n---SPORTS APP---");
                Console.WriteLine("0) Exit.");
                Console.WriteLine("1) Add a Team.");
                Console.WriteLine("2) View Teams.");
                Console.WriteLine("3) Add a Match.");
                Console.WriteLine("4) View Standings.");
                Console.WriteLine("5) View Matches.");
                Console.WriteLine("6) Advance Options.");

                choice = AreNumbers();

                if(choice<0 || choice>6)
                {
                    Console.WriteLine("The choice that you entered is INVALID!");
                    continue;
                }

                switch(choice)
                {
                    case 0:
                        return;
                    case 1:
                        AddTeam();                  
                        break;
                    case 2:
                        ViewTeams();
                        break;
                    case 3:
                        AddMatch();
                        break;
                    case 4:
                        ShowStandings();
                        break;
                    case 5:
                        ViewMatches();
                        break;
                    case 6:   
                        AdvanceOptions();
                        break;
                    default:
                        Console.WriteLine("Wrong choice!\n");
                        break;                                   
                }
            }
        }

        private void AddTeam(){
            Console.WriteLine("\n---ADD A TEAM---");
            string tname;
            while(true)
            {
                tname = AreLetters();
                if(teams.Any(t => t.Name.Equals(tname, StringComparison.OrdinalIgnoreCase))){
                    Console.WriteLine("This team already exists!\n");
                    continue;
                }
                break;
            }
            teams.Add(new Team(tname));
            Console.WriteLine("Team has been added!\n");
            SaveData();
        }

        private void ViewTeams(){ 
            Console.WriteLine("\n---ADDED TEAMS---");
            int i;
            if(teams.Count==0){
                Console.WriteLine("No Teams Added Yet!");
            }
            else{
                for(i=0; i<teams.Count; i++)
                {
                    Console.WriteLine($"{i+1}. {teams[i].Name}");
                }
            }
        }

        private void AddMatch(){
            if(teams.Count<2)
            {
                Console.WriteLine("You need at least TWO Teams to add a Match\n");
                return;
            }

            Console.WriteLine("\n---ADD A MATCH---");

            for(int i=0; i<teams.Count; i++)
            {
                Console.WriteLine($"{i+1}. {teams[i].Name}");
            } 

            int homeIndex=0;
            int awayIndex=0;

            while(true)
            {
                Console.WriteLine("Select Home Team.");
                homeIndex = AreNumbers();
                if(homeIndex>=1 && homeIndex<=teams.Count){
                    break;
                }
                Console.WriteLine("Your choice is Invalid.\n");
            }

            while(true)
            {
                Console.WriteLine("Select Away Team.");
                awayIndex = AreNumbers();
                if(awayIndex>=1 && awayIndex<=teams.Count){
                    if(awayIndex==homeIndex){
                        Console.WriteLine("You cannot have the SAME Team both at Home AND Away at the same time!\n");
                        continue;
                    }
                    break;
                }
            }

            int homeScore=0, awayScore=0;

            while(true)
            {
                Console.WriteLine($"Enter Score for {teams[homeIndex-1].Name}: ");
                homeScore = AreNumbers();
                if(homeScore>=0){
                    break;
                }
                Console.WriteLine("The Score Cannot be Negative!");
            }

            while(true)
            {
                Console.WriteLine($"Enter Score for {teams[awayIndex-1].Name}: ");
                awayScore = AreNumbers();
                if(awayScore>=0){
                    break;
                }
                Console.WriteLine("The Score Cannot be Negative!");
            }

            Team homeTeam = teams[homeIndex-1];
            Team awayTeam = teams[awayIndex-1];

            Match match = new Match(homeTeam, awayTeam, homeScore, awayScore);
            match.ApplyResult();
            matches.Add(match);

            Console.WriteLine($"Match Recorded: {homeTeam.Name} {homeScore} - {awayScore} {awayTeam.Name}\n");
            SaveData();
            return;
        }

        private void ShowStandings(){
            if(teams.Count==0){
                Console.WriteLine("No Teams Added Yet!");
            }
            else{
                Console.WriteLine("\n---STANDINGS---");
                OrderedTable(teams);
            }
        }

        private void AdvanceOptions(){
            if(teams.Count==0){
                Console.WriteLine("No Teams Added Yet!");
                return;
            }
            else{
                bool flag = true;
                while(flag){
                    Console.WriteLine("\n---ADVANCE OPTIONS---");
                    Console.WriteLine("0) Back to Menu.");
                    Console.WriteLine("1) Edit Team Name.");
                    Console.WriteLine("2) Edit Matches");
                    Console.WriteLine("3) Delete Team");
                    Console.WriteLine("4) Reset League");

                    int inputAo;

                    while(true){
                        inputAo = AreNumbers();
                        if(inputAo>=0 && inputAo<=4){
                            break;
                        }
                        Console.WriteLine("Your Choice is Invalid.");
                    }
                    switch(inputAo){
                        case 0:
                            flag = false;
                            break;

                        case 1:
                            EditTeam();
                            break;
                        case 2:
                            EditMatch();
                            break;
                        case 3:
                            DeleteTeam();
                            break;
                        case 4:
                            ResetLeague();
                            break;
                        default:
                            Console.WriteLine("Wrong choice!\n");
                            break;
                    }
                }
            }
        }

        private void EditTeam(){

            Console.WriteLine("\n---EDIT TEAM---");                        
            int teamIndex;
            bool flag = true;
            while(flag){
                Console.WriteLine("Select the Team that you want to Edit.");
                                    
                for(int i=0; i<teams.Count; i++){
                    Console.WriteLine($"{i+1}. {teams[i].Name}");
                }
                teamIndex = AreNumbers();

                if(teamIndex >= 1 && teamIndex <= teams.Count){
                    Team editTeam = teams[teamIndex - 1];
                    Console.WriteLine("Edit Team Name");
                    Console.WriteLine($"Current Team Name: {teams[teamIndex-1].Name}");
                    Console.WriteLine("Enter the NEW Name of the Team.");
                    string newTeamName = AreLetters();
                    if(teams.Any(t =>t != editTeam && t.Name.Equals(newTeamName, StringComparison.OrdinalIgnoreCase))){
                        Console.WriteLine("This team already exists!");
                        continue;
                    }
                    editTeam.Name=newTeamName;        
                    Console.WriteLine($"Team {editTeam.Name} updated succesfully!\n");
                    flag = false;
                }
                else{
                    Console.WriteLine("Wrong choice.\n");    
                }
            }
            SaveData(); 
        }

        private void EditMatch(){
            Console.WriteLine("\n---EDIT MATCH---");
            if(matches.Count==0){
                Console.WriteLine("No Matches Added Yet!");
            }
            else{
                int matchIndex;
                bool flag = true;
                while(flag){
                    ShowMatches();
                    Console.WriteLine("Select the Match that you Want to Edit.");
                    matchIndex=AreNumbers();
                    if(matchIndex>=1 && matchIndex<=matches.Count){
                        Match editMatch = matches[matchIndex-1];
                        Console.WriteLine($"\nChoosed Match to Edit: {editMatch.HomeTeam.Name} {editMatch.HomeScore} - {editMatch.AwayScore} {editMatch.AwayTeam.Name}");
                        Console.WriteLine("Select what you Want to Edit from the Match.");
                        Console.WriteLine("0) Back to Previous Menu.");
                        Console.WriteLine("1) Change The Score of the HomeTeam.");
                        Console.WriteLine("2) Change The Score of the AwayTeam.");
                        int choice = AreNumbers();
                        switch(choice){
                            case 0:
                                flag=false;
                                break;
                            case 1:
                                HomeTeamScoreEdit(editMatch);
                                flag=false;
                                break;
                            case 2:
                                AwayTeamScoreEdit(editMatch);
                                flag=false;
                                break;
                            default:
                                Console.WriteLine("Wrong Choice.");
                                break;
                        }
                    }
                    else{
                        Console.WriteLine("Wrong Match Choice.");
                    }
                }
            }
        }

        private void DeleteTeam(){
            Console.WriteLine("---DELETE TEAM---");
            if(teams.Count==0){
                Console.WriteLine("You Need Atleast ONE Team Added!");
                return;
            }
            Console.WriteLine("Select the Team that you want to DELETE!");
            for(int i=0; i<teams.Count; i++){
                Console.WriteLine($"{i+1}. {teams[i].Name}  W: {teams[i].Wins} D: {teams[i].Draws} L: {teams[i].Losses}");
            }
            int delChoice;
            bool delch = true;
            while(delch){
                delChoice = AreNumbers();
                if(delChoice>=1 && delChoice<=teams.Count){
                    Team teamToDelete = teams[delChoice - 1];
                    Console.WriteLine($"Are you SURE that you want to DELETE: {teamToDelete.Name} (Answer y/n)");
                    string answer;
                    bool ans = true;
                    while(ans){
                        answer = AreLetters();
                        if(answer.ToLower()=="y"){
                            var matchesToRemove = matches.Where(m => m.HomeTeam == teamToDelete || m.AwayTeam == teamToDelete).ToList();

                            foreach(var m in matchesToRemove){

                                m.UndoResult();
                                                        
                            }
                            matches.RemoveAll(m => m.HomeTeam == teamToDelete || m.AwayTeam == teamToDelete);
                            teams.RemoveAt(delChoice - 1);
                            Console.WriteLine("The Team Deleted Successfully!");
                            ans=false;
                        }
                        else if(answer.ToLower()=="n"){
                            Console.WriteLine("Delete Cancelled.");
                            ans=false;
                        }
                        else{
                            Console.WriteLine("Your Answer is Invalid.");
                        }
                    }
                    delch = false;
                }
                else{
                    Console.WriteLine("Your Choice is Invalid.");
                }
            }
            SaveData();
        }

        private void ResetLeague(){
            Console.WriteLine("\n---RESET LEAGUE---");
            if(teams.Count==0 && matches.Count==0){
                Console.WriteLine("No Data to Reset.");
            }
            else{
                while(true){
                    Console.WriteLine("Are you Sure that you Want to Reset the League?(y/n)");
                    string answer = AreLetters();
                    if(answer.ToLower()=="y"){
                        matches.Clear();
                        foreach(var team in teams){
                            team.UpdateRecord(0, 0, 0);
                            team.GoalsFor = 0;
                            team.GoalsAgainst = 0;
                            team.GamesPlayed = 0;
                        }
                        Console.WriteLine("League has been RESETED successfully!");
                        break;
                    }
                    else if(answer.ToLower()=="n"){
                        Console.WriteLine("Reset Cancelled.");
                        break;
                    }
                    else{
                        Console.WriteLine("Wrong Answer.");
                    }
                }
            }
            SaveData();
        }

        private void HomeTeamScoreEdit(Match match){
            Console.WriteLine("\n---HOME TEAM SCORE EDIT---");
            Console.WriteLine($"\nChoosed Match to Edit: {match.HomeTeam.Name} {match.HomeScore} - {match.AwayScore} {match.AwayTeam.Name}");
            match.UndoResult();
            int newScore;
            while(true)
            {
                Console.WriteLine($"Enter New Score for {match.HomeTeam.Name}: ");
                newScore = AreNumbers();
                if(newScore >= 0)
                    break;
                Console.WriteLine("Score cannot be negative!");
            }
            Match updatedMatch = new Match(match.HomeTeam, match.AwayTeam, newScore, match.AwayScore);
            updatedMatch.ApplyResult();

            matches[matches.IndexOf(match)] = updatedMatch;
            Console.WriteLine("Home Team Score Updated Successfully!");
            SaveData();
        }

        private void AwayTeamScoreEdit(Match match){
            Console.WriteLine("\n---AWAY TEAM SCORE EDIT---");
            Console.WriteLine($"\nChoosed Match to Edit: {match.HomeTeam.Name} {match.HomeScore} - {match.AwayScore} {match.AwayTeam.Name}");
            match.UndoResult();
            int newScore;
            while(true)
            {
                Console.WriteLine($"Enter New Score for {match.AwayTeam.Name}: ");
                newScore = AreNumbers();
                if(newScore >= 0)
                    break;
                Console.WriteLine("Score cannot be negative!");
            }
            Match updatedMatch = new Match(match.HomeTeam, match.AwayTeam, match.HomeScore, newScore);
            updatedMatch.ApplyResult();

            matches[matches.IndexOf(match)] = updatedMatch;
            Console.WriteLine("Away Team Score Updated Successfully!");
            SaveData();
        }

        private void ViewMatches(){
            Console.WriteLine("\n---VIEW MATCHES---");
            if(matches.Count==0){
                Console.WriteLine("No Matches Added Yet!");
            }
            else{
                ShowMatches();
            }
        }

        static void OrderedTable(List<Team> teams){
            var sortedTeams = teams
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalsDifference)
                .ThenByDescending(t => t.GoalsFor)
                .ThenBy(t => t.Name)
                .ToList();

            foreach(var team in sortedTeams){
                team.Show();
            }
        }


        public void ShowMatches(){
            for(int i=0; i<matches.Count; i++){
                Console.WriteLine($"{i+1}. {matches[i].HomeTeam.Name} {matches[i].HomeScore} - {matches[i].AwayScore} {matches[i].AwayTeam.Name}"); 
            }
        }

        static int AreNumbers(){
            int input;
            while(true){
                string Input = Console.ReadLine();
                if(!int.TryParse(Input, out input)){
                    Console.WriteLine("Please Enter Only Numbers!\n");
                }
                else{
                    return input;
                }
            }
        }

        static string AreLetters(){
            while(true){
                string input = Console.ReadLine();
                if(input.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)) && !string.IsNullOrWhiteSpace(input)){
                    return(input);
                }
                else{
                    Console.WriteLine("Please enter only Letters!\n");
                }
            }
        }

        private void SaveData(){
            var data = new{
                Teams = teams,
                Matches = matches.Select(m => new {
                    HomeTeamName = m.HomeTeam.Name,
                    AwayTeamName = m.AwayTeam.Name,
                    HomeScore = m.HomeScore,
                    AwayScore = m.AwayScore
                }).ToList()
            };

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("leagueData.json", json);
        }

        private void LoadData(){
            if (!File.Exists("leagueData.json")) return;

            string json = File.ReadAllText("leagueData.json");
            var data = JsonSerializer.Deserialize<LeagueData>(json);

            if (data != null){
        
                teams.Clear();
                foreach(var t in data.Teams)
                {
                    teams.Add(new Team(t.Name)
                    {
                        Wins = t.Wins,
                        Draws = t.Draws,
                        Losses = t.Losses,
                        Points = t.Points,
                        GoalsFor = t.GoalsFor,
                        GoalsAgainst = t.GoalsAgainst,
                        GamesPlayed = t.GamesPlayed
                    });
                }

                matches.Clear();
                foreach(var m in data.Matches){
                    Team home = teams.FirstOrDefault(t => t.Name == m.HomeTeamName);
                    Team away = teams.FirstOrDefault(t => t.Name == m.AwayTeamName);
                    if(home != null && away != null){
                        Match match = new Match(home, away, m.HomeScore, m.AwayScore);
                        matches.Add(match);
                    }
                }
            }
        }
    }

    public class LeagueData{
        public List<Team> Teams { get; set; }
        public List<MatchJson> Matches { get; set; }
    }

    public class MatchJson{
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
    }

}