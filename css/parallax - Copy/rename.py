# Pythono3 code to rename multiple 
# files in a directory or folder
  
# importing os module
import os
import sys
os.chdir(os.path.dirname(sys.argv[0]))

# Function to rename multiple files
def main():
  
    for count, filename in enumerate(os.listdir(".")):
        print(filename)
          
        # rename() function will
        # rename all the files
        if ".webp" not in filename.lower():
            continue
        os.rename("./"+filename, "./"+filename.replace(".PNG", ""))
  

main()
for i in range(9):
    x = f"let layer_{i} = document.getElementById('Layer {i}')"
    print(x)