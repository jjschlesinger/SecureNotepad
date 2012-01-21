using System;
using System.Collections.Generic;
using System.Linq;
using System.Json;

namespace SecureNotepad.Core.Net.SkyDrive
{
    public class SkyDriveClient : LiveClient
    {
        private const string SKYDRIVE_ROOT_PATH = "/me/skydrive";

        public SkyDriveClient(string accessToken) : base(accessToken)
        {
        	
        }

        public void GetFolderItemsAsync(Action<IEnumerable<BaseItem>> callback, string folderId = null)
        {
            string path;

            if (folderId == null)
                path = SKYDRIVE_ROOT_PATH;
            else
                path = "/" + folderId;

            path = path + "/files";
            
            Execute(t =>
            {
                //t.Wait();
                var data = JsonArray.Parse(t.Result)["data"] as JsonArray;

                var items = new List<BaseItem>();
                foreach (var j in data)
                {
                    BaseItem i;
                    var type = j["type"].ReadAs<String>();
                    switch (type)
                    {
                        case "file":
                        case "photo":
                        case "video":
                            i = new FileItem();
                            break;
                        default:
                            i = new FolderItem { ChildrenCount = j["count"].ReadAs<Int32>(), UploadLocation = j["upload_location"].ReadAs<String>() };
                            break;
                    }

                    i.Id = j["id"].ReadAs<String>();
                    i.Name = j["name"].ReadAs<String>();
                    i.Description = j["description"] != null ? j["description"].ReadAs<String>() : null;
                    i.Created = j["created_time"] != null ? (DateTime?)j["created_time"].ReadAs<DateTime>() : null;
                    i.Updated = j["updated_time"] != null ? (DateTime?)j["updated_time"].ReadAs<DateTime>() : null;

                    items.Add(i);
                }

                callback(items);

            }, path, "GET");
        }
    }
}
