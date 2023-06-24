namespace CommandsService.Dto;

public class CommandReadDto
{
    public int Id { get; set; }
    public string CommandName { get; set; }
    public string CommandLine { get; set; }
    public int PlatformId { get; set; }
}