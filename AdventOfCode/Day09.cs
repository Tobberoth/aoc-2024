using System.Text;

namespace AdventOfCode;

public class Day09: BaseDay {
    public string Input { get; set; }
    public List<int> Drive { get; set; } = [];
    public Day09() {
        Input = File.ReadAllText(InputFilePath);
        var currentId = 0;
        for (var i = 0; i < Input.Length; i++) {
            if (i % 2 == 0) {
                for (var x = 0; x < int.Parse(Input[i].ToString()); x++) {
                    Drive.Add(currentId);
                }
                currentId++;
            } else {
                for (var x = 0; x < int.Parse(Input[i].ToString()); x++) {
                    Drive.Add(-1);
                }
            }
        }
    }

    private List<int> CompactBlocks() {
        List<int> compactDrive = [..Drive];
        for (var i = Drive.Count-1; i > 0; i--) {
            if (Drive[i] != -1) {
                var place = compactDrive.IndexOf(-1);
                if (place >= i) break;
                compactDrive[place] = Drive[i];
                compactDrive[i] = -1;
            }
        }
        return compactDrive;
    }

    private long CalcChecksum(List<int> compactDrive) {
        long sum = 0;
        for (var i = 0; i < compactDrive.Count; i++) {
            var id = compactDrive[i];
            if (id == -1)
                break;
            sum += i * id;
        }
        return sum;
    }

    public override ValueTask<string> Solve_1() {
        var compactDrive = CompactBlocks();
        return new($"{CalcChecksum(compactDrive)}");
    }

    public override ValueTask<string> Solve_2() {
        var advDrive = new AdvancedDrive(Drive);
        for (var i = advDrive.MaxID; i > 0; i--) {
            var freeIndex = advDrive.FindFirstFreeSlotOfSize(advDrive.GetFile(i).Length);
            if (freeIndex > -1 && freeIndex < advDrive.GetFileIndex(advDrive.GetFile(i)))
                advDrive.Move(i, freeIndex);
        }
        return new($"{advDrive.CalcCheckSum()}");
    }

    public class AdvancedDrive {
        public List<Slot> Slots { get; set; } = [];
        public int MaxID => Slots.Max(s => s.ID);
        public AdvancedDrive(List<int> drive) {
            Slot currentSlot = new Slot{ Length = 0, ID = -2 };
            for (var i = 0; i < drive.Count; i++) {
                var id = drive[i];
                if (currentSlot.ID != id) {
                    if (currentSlot.ID != -2)
                        Slots.Add(currentSlot);
                    currentSlot = new Slot { Length = 1, ID = id };
                } else {
                    currentSlot = new Slot { Length = ++currentSlot.Length, ID = currentSlot.ID };
                }
            }
            Slots.Add(currentSlot);
        }

        public Slot GetFile(int id) {
            if (id == -1)
                throw new InvalidOperationException("Can't get free space");
            return Slots.FirstOrDefault(s => s.ID == id);
        }

        public int FindFirstFreeSlotOfSize(int size) {
            for (var i = 0; i < Slots.Count; i++) {
                if (Slots[i].ID == -1 && Slots[i].Length >= size)
                    return i;
            }
            return -1;
        }

        public int GetFileIndex(Slot file) {
            if (file.ID == -1)
                throw new InvalidOperationException("Can't find index of free space");
            for (var i = 0; i < Slots.Count; i++) {
                if (Slots[i] == file)
                    return i;
            }
            return -1;
        }

        public override string ToString() {
            StringBuilder sb = new();
            for (var i = 0; i < Slots.Count; i++) {
                for (var x = 0; x < Slots[i].Length; x++) {
                    sb.Append(Slots[i].ID == -1 ? "." : Slots[i].ID.ToString());
                }
            }
            return sb.ToString();
        }

        internal void Move(int i, int freeIndex) {
            var file = GetFile(i);
            var fileIndex = GetFileIndex(file);
            var fileLength = file.Length;
            var sizeDiff = Slots[freeIndex].Length - fileLength;
            if (sizeDiff > 0) {
                Slots.Insert(freeIndex, new Slot { ID = i, Length = fileLength });
                Slots[freeIndex+1] = new Slot { ID = -1, Length = sizeDiff };
                fileIndex++;
            } else {
                Slots[freeIndex] = new Slot { ID = i, Length = fileLength };
            }

            Slot preSource = default;
            if (fileIndex - 1 > -1)
                preSource = Slots[fileIndex-1];
            Slot afterSource = default;
            if ((fileIndex + 1) < Slots.Count)
                afterSource = Slots[fileIndex+1];
            if (preSource.ID == -1 && afterSource?.ID == -1) {
                Slots[fileIndex-1] = new Slot { ID = -1, Length = preSource.Length + fileLength + afterSource.Length };
                Slots.RemoveAt(fileIndex);
                Slots.RemoveAt(fileIndex);
            } else if (preSource?.ID == -1) {
                Slots[fileIndex-1] = new Slot { ID = -1, Length = preSource.Length + fileLength };
                Slots.RemoveAt(fileIndex);
            } else if (afterSource?.ID == -1) {
                Slots[fileIndex+1] = new Slot { ID = -1, Length = afterSource.Length + fileLength };
                Slots.RemoveAt(fileIndex);
            } else {
                Slots[fileIndex] = new Slot { ID = -1, Length = fileLength };
            }
        }

        public long CalcCheckSum() {
            long sum = 0;
            var i = 0;
            foreach (var slot in Slots) {
                if (slot.ID == -1) {
                    i += slot.Length;
                    continue;
                }
                for (var x = 0; x < slot.Length; x++) {
                    sum += i * slot.ID;
                    i++;
                }
            }
            return sum;
        }
    }

    public record Slot {
        public int Length { get; set; }
        public int ID { get; set; } = -1;
    }
}