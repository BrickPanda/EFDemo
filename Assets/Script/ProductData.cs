using System.Collections;
using System.Collections.Generic;
public class Audio
{
    /// <summary>
    /// 
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string url { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int size { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string sha1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string mimeType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int width { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int height { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string language { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string title { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int duration { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string thumbnails { get; set; }

}

public class Item
{
    /// <summary>
    /// 
    /// </summary>
    public string type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string text { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string prompt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string hideText { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string image { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Audio audio { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string pdf { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string video { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string audioLocal { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string academic { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string showMode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string subtitles { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int rowIndex { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int colIndex { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool lockedPosition { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string expected { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string speaker { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string table { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int startRow { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int endRow { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int startCol { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int endCol { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string cells { get; set; }

}

public class Body
{
    /// <summary>
    /// 
    /// </summary>
    public Item item { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string version { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<TagsItem> tags { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string skillSet { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<string> tests { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<OptionsItem> options { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string layoutMode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<List<string>> answers { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string mode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string background { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string asrEngine { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int hintTime { get; set; }

}

public class StimulusItem
{
    /// <summary>
    /// 
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Body Body { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Tags { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string stimulusOfQuestion { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double questionAnchor { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double answerAnchor { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool isForModeling { get; set; }

}

public class TagsItem
{
    /// <summary>
    /// 
    /// </summary>
    public string compassTags { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string subSkillSet { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string vocabulary { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int rows { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int cols { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string primary_skill_set { get; set; }
}

public class ImageData
{
    /// <summary>
    /// 
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string url { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int size { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string sha1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string mimeType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int width { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int height { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string language { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string title { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int duration { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string thumbnails { get; set; }

}

public class OptionsItem
{
    /// <summary>
    /// 
    /// </summary>
    public string type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string text { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string prompt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string hideText { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ImageData image { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string audio { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string pdf { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string video { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string audioLocal { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string academic { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string showMode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string subtitles { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int rowIndex { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int colIndex { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool lockedPosition { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string expected { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string speaker { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string table { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int startRow { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int endRow { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int startCol { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int endCol { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string cells { get; set; }

}

public class StimulusOfQuestion
{
    /// <summary>
    /// 
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Body Body { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Tags { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string stimulusOfQuestion { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double questionAnchor { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double answerAnchor { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool isForModeling { get; set; }

}

public class QuestionsItem
{
    /// <summary>
    /// 
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Body Body { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Tags { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public StimulusOfQuestion stimulusOfQuestion { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double questionAnchor { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double answerAnchor { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool isForModeling { get; set; }

}

public class MappingsItem
{
    /// <summary>
    /// 
    /// </summary>
    public string s { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string q { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string anchor { get; set; }

}

public class Tags
{
    /// <summary>
    /// 
    /// </summary>
    public List<string> primary_skill_set { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string skillType { get; set; }

}

public class Activity
{
    /// <summary>
    /// 
    /// </summary>
    public List<StimulusItem> Stimulus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<QuestionsItem> Questions { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Tags { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string activityQuestionMd5 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string part { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string ContentId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string ContentRevision { get; set; }

}



public class ProductData
{
    /// <summary>
    /// 
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string BookName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string UnitName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string LessonName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Activity Activity { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Owner { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string CreatedStamp { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string LastUpdatedStamp { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int State { get; set; }
}
