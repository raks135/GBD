
class Calculate:
    def __init__(self,a,b):
        self.a=a
        self.b=b

    def add(self):
        return self.a+self.b

obj=Calculate(5,3)
result=obj.add()
print(f"The result is {result}.")        
