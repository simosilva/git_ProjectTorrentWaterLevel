clc
close all
clear all

n=1;                                    %number of measurements
samples=306;                            %number of samples taken
sample_time=0.65e-5;                    %sample time of arduino
R=9.87e3;                               %measured value of R
C=0.1e-6                                %capacitance
tau=R*C                                 %estimated tau

comPort = '/dev/cu.usbmodem1411';       %com port of arduino
s = serial(comPort)     
fopen(s)                                %open it

for j=1:n                              %times the number of measurements
    while(s.BytesAvailable == 0)        %wait until there is some data
    end

    a=fscanf(s)                         %take the "start"
    for i=1:samples
       b(j,i)=fscanf(s, '%d');          %take the real values that are #samples
    end
    a=fscanf(s)                         %take the "end"
end

fclose(s)                               %close the com port!!

    figure
for j=1:n                               %plot n graphs 
 
    subplot(n,2,j+(j-1))
    plot(b(j,:))                        %each line is the n-th measure
    title(['raw data # ', num2str(j)]); 
    xlabel('Sample #','FontSize',12,'FontWeight','bold');
    ylabel('ADC value','FontSize',12,'FontWeight','bold');
    hold on
    grid on


    subplot(n,2,j+(j-1)+1)
    data=log(b(j,:)/1024); 
    [slope offset]=linear_regression((0:sample_time:((samples-1)*sample_time)), data); %linear regression of data in a range of 0->samples*sample time
    title(['linear regression # ', num2str(j)]);
    ylabel('Logarithmic scaling','FontSize',12,'FontWeight','bold')
    xlabel('Time(s)','FontSize',12,'FontWeight','bold')
    C_m2(j)=-1/(slope*R);               %calculate C -> slope=-1/RC
    C_m2(j)=C_m2(j)*1e6;
   
   end
